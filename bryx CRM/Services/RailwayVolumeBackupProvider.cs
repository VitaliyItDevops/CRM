using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace bryx_CRM.Services;

/// <summary>
/// Railway Volume провайдер бэкапов
/// Использует pg_dump для создания бэкапов и сохраняет их в Railway Volume
/// Volume автоматически бэкапится Railway (двойная защита!)
/// </summary>
public class RailwayVolumeBackupProvider : IBackupProvider
{
    private readonly ILogger<RailwayVolumeBackupProvider> _logger;
    private readonly IConfiguration _configuration;
    private readonly BackupOptions _options;
    private readonly string _volumePath;

    public string ProviderType => "Railway Volume";
    public string Description => "Бэкапы в Railway Volume с автоматическим бэкапом самого Volume (двойная защита)";

    public RailwayVolumeBackupProvider(
        ILogger<RailwayVolumeBackupProvider> logger,
        IConfiguration configuration,
        IOptions<BackupOptions> options)
    {
        _logger = logger;
        _configuration = configuration;
        _options = options.Value;

        // Путь к Railway Volume (можно настроить через переменную окружения)
        _volumePath = Environment.GetEnvironmentVariable("RAILWAY_VOLUME_MOUNT_PATH")
            ?? _options.BackupDirectory;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Проверяем, что мы в Railway окружении
            var isRailway = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT"));

            if (!isRailway)
            {
                return false;
            }

            // Проверяем доступность Volume пути
            if (!Directory.Exists(_volumePath))
            {
                try
                {
                    Directory.CreateDirectory(_volumePath);
                    _logger.LogInformation("📁 [Railway] Создана директория в Volume: {Path}", _volumePath);
                }
                catch
                {
                    return false;
                }
            }

            // Проверяем, можем ли мы использовать pg_dump через Docker
            // В Railway мы можем использовать официальный PostgreSQL Docker образ с утилитами
            return true;
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
            if (!Directory.Exists(_volumePath))
            {
                Directory.CreateDirectory(_volumePath);
                _logger.LogInformation("📁 [Railway] Создана директория для бекапов в Volume: {Directory}", _volumePath);
            }

            // Получаем connection string из DATABASE_URL (Railway формат)
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DATABASE_URL не найден");
            }

            // Парсим connection string
            var (host, port, database, username, password) = ParseConnectionString(connectionString);

            // Генерируем имя файла бекапа
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"railway_backup_{database}_{timestamp}.sql";
            var backupFilePath = Path.Combine(_volumePath, backupFileName);

            _logger.LogInformation("🔄 [Railway] Начинаем создание бекапа в Volume: {Database}", database);

            // Используем pg_dump напрямую (установлен через nixpacks.toml)
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
                _logger.LogError("❌ [Railway] Ошибка при создании бекапа: {Error}", error);
                throw new Exception($"pg_dump завершился с ошибкой: {error}");
            }

            // Проверяем размер файла
            var fileInfo = new FileInfo(backupFilePath);
            var fileSizeMb = fileInfo.Length / 1024.0 / 1024.0;

            _logger.LogInformation("✅ [Railway] Бекап успешно создан в Volume: {FileName} ({Size:F2} МБ)", backupFileName, fileSizeMb);
            _logger.LogInformation("🛡️ [Railway] Volume будет автоматически забэкаплен Railway");

            return backupFilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Railway] Критическая ошибка при создании бекапа");
            throw;
        }
    }


    public async Task<bool> RestoreBackupAsync(string backupFilePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(backupFilePath))
            {
                throw new FileNotFoundException($"Файл бекапа не найден в Volume: {backupFilePath}");
            }

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? _configuration.GetConnectionString("DefaultConnection");

            var (host, port, database, username, password) = ParseConnectionString(connectionString);

            _logger.LogInformation("🔄 [Railway] Начинаем восстановление из бекапа в Volume: {BackupFile}", Path.GetFileName(backupFilePath));

            // Используем pg_restore напрямую (установлен через nixpacks.toml)
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
                _logger.LogError("❌ [Railway] Ошибка при восстановлении бекапа: {Error}", error);
                throw new Exception($"pg_restore завершился с ошибкой: {error}");
            }

            _logger.LogInformation("✅ [Railway] База данных успешно восстановлена из бекапа");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Railway] Критическая ошибка при восстановлении бекапа");
            throw;
        }
    }


    public Task<List<BackupInfo>> GetBackupListAsync()
    {
        try
        {
            if (!Directory.Exists(_volumePath))
            {
                return Task.FromResult(new List<BackupInfo>());
            }

            var backupFiles = Directory.GetFiles(_volumePath, "*backup_*.sql")
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
            _logger.LogError(ex, "❌ [Railway] Ошибка при получении списка бекапов из Volume");
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
                _logger.LogInformation("🗑️ [Railway] Бекап удалён из Volume: {FileName}", Path.GetFileName(backupFilePath));
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Railway] Ошибка при удалении бекапа из Volume: {FilePath}", backupFilePath);
            return Task.FromResult(false);
        }
    }

    private (string host, string port, string database, string username, string password) ParseConnectionString(string connectionString)
    {
        // Railway использует формат postgres://
        if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            return (uri.Host, uri.Port.ToString(), uri.AbsolutePath.TrimStart('/'), userInfo[0], userInfo.Length > 1 ? userInfo[1] : "");
        }
        else
        {
            // Npgsql формат (для локальной разработки)
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
