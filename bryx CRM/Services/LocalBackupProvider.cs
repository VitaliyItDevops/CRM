using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace bryx_CRM.Services;

/// <summary>
/// Локальный провайдер бэкапов, использующий pg_dump/pg_restore
/// Работает на локальной машине где установлены PostgreSQL утилиты
/// </summary>
public class LocalBackupProvider : IBackupProvider
{
    private readonly ILogger<LocalBackupProvider> _logger;
    private readonly IConfiguration _configuration;
    private readonly BackupOptions _options;

    public string ProviderType => "Local (pg_dump)";
    public string Description => "Локальные бэкапы через утилиты PostgreSQL (требуется установка pg_dump/pg_restore)";

    public LocalBackupProvider(
        ILogger<LocalBackupProvider> logger,
        IConfiguration configuration,
        IOptions<BackupOptions> options)
    {
        _logger = logger;
        _configuration = configuration;
        _options = options.Value;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Проверяем наличие pg_dump
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            await process.WaitForExitAsync(cancellationToken);

            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Создаём директорию для бекапов если её нет
            if (!Directory.Exists(_options.BackupDirectory))
            {
                Directory.CreateDirectory(_options.BackupDirectory);
                _logger.LogInformation("📁 Создана директория для бекапов: {Directory}", _options.BackupDirectory);
            }

            // Получаем connection string
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string не найден");
            }

            // Парсим connection string
            var (host, port, database, username, password) = ParseConnectionString(connectionString);

            // Генерируем имя файла бекапа
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"backup_{database}_{timestamp}.sql";
            var backupFilePath = Path.Combine(_options.BackupDirectory, backupFileName);

            _logger.LogInformation("🔄 [Local] Начинаем создание бекапа базы данных: {Database}", database);

            // Создаём процесс pg_dump
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = $"-h {host} -p {port} -U {username} -d {database} -F c -f \"{backupFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Устанавливаем пароль через переменную окружения
            processStartInfo.Environment["PGPASSWORD"] = password;

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = await process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                _logger.LogError("❌ [Local] Ошибка при создании бекапа: {Error}", error);
                throw new Exception($"pg_dump завершился с ошибкой: {error}");
            }

            // Проверяем размер файла
            var fileInfo = new FileInfo(backupFilePath);
            var fileSizeMb = fileInfo.Length / 1024.0 / 1024.0;

            _logger.LogInformation("✅ [Local] Бекап успешно создан: {FileName} ({Size:F2} МБ)", backupFileName, fileSizeMb);

            return backupFilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Local] Критическая ошибка при создании бекапа");
            throw;
        }
    }

    public async Task<bool> RestoreBackupAsync(string backupFilePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(backupFilePath))
            {
                throw new FileNotFoundException($"Файл бекапа не найден: {backupFilePath}");
            }

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? _configuration.GetConnectionString("DefaultConnection");

            var (host, port, database, username, password) = ParseConnectionString(connectionString);

            _logger.LogInformation("🔄 [Local] Начинаем восстановление базы данных из бекапа: {BackupFile}", Path.GetFileName(backupFilePath));

            // Создаём процесс pg_restore
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "pg_restore",
                Arguments = $"-h {host} -p {port} -U {username} -d {database} -c \"{backupFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            processStartInfo.Environment["PGPASSWORD"] = password;

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = await process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                _logger.LogError("❌ [Local] Ошибка при восстановлении бекапа: {Error}", error);
                throw new Exception($"pg_restore завершился с ошибкой: {error}");
            }

            _logger.LogInformation("✅ [Local] База данных успешно восстановлена из бекапа");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Local] Критическая ошибка при восстановлении бекапа");
            throw;
        }
    }

    public Task<List<BackupInfo>> GetBackupListAsync()
    {
        try
        {
            if (!Directory.Exists(_options.BackupDirectory))
            {
                return Task.FromResult(new List<BackupInfo>());
            }

            var backupFiles = Directory.GetFiles(_options.BackupDirectory, "backup_*.sql")
                .Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    return new BackupInfo
                    {
                        FileName = fileInfo.Name,
                        FilePath = filePath,
                        CreatedAt = fileInfo.CreationTime,
                        SizeMb = fileInfo.Length / 1024.0 / 1024.0,
                        ProviderType = ProviderType
                    };
                })
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return Task.FromResult(backupFiles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Local] Ошибка при получении списка бекапов");
            return Task.FromResult(new List<BackupInfo>());
        }
    }

    public Task<bool> DeleteBackupAsync(string backupFilePath)
    {
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Delete(backupFilePath);
                _logger.LogInformation("🗑️ [Local] Бекап удалён: {FileName}", Path.GetFileName(backupFilePath));
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Local] Ошибка при удалении бекапа: {FilePath}", backupFilePath);
            return Task.FromResult(false);
        }
    }

    private (string host, string port, string database, string username, string password) ParseConnectionString(string connectionString)
    {
        // Если это Railway формат (postgres:// или postgresql://)
        if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            return (uri.Host, uri.Port.ToString(), uri.AbsolutePath.TrimStart('/'), userInfo[0], userInfo[1]);
        }
        else
        {
            // Npgsql формат
            var parts = connectionString.Split(';')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToDictionary(
                    p => p.Split('=')[0].Trim().ToLower(),
                    p => p.Split('=')[1].Trim()
                );

            return (
                parts.GetValueOrDefault("host", "localhost"),
                parts.GetValueOrDefault("port", "5432"),
                parts.GetValueOrDefault("database", "bryx_crm"),
                parts.GetValueOrDefault("username", "postgres"),
                parts.GetValueOrDefault("password", "")
            );
        }
    }
}
