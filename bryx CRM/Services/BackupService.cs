using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using Npgsql;

namespace bryx_CRM.Services;

public class BackupOptions
{
    public string BackupDirectory { get; set; } = "Backups";
    public int MaxBackupsToKeep { get; set; } = 10;
    public bool CompressBackups { get; set; } = true;
    public bool UseSqlDump { get; set; } = true; // true = SQL dump (работает везде), false = pg_dump (требует утилиты)
}

public class BackupService
{
    private readonly ILogger<BackupService> _logger;
    private readonly IEnumerable<IBackupProvider> _providers;
    private readonly BackupOptions _options;
    private IBackupProvider? _activeProvider;

    public string CurrentProviderType => _activeProvider?.ProviderType ?? "Не определён";
    public string CurrentProviderDescription => _activeProvider?.Description ?? "";

    public BackupService(
        ILogger<BackupService> logger,
        IEnumerable<IBackupProvider> providers,
        IOptions<BackupOptions> options)
    {
        _logger = logger;
        _providers = providers;
        _options = options.Value;
    }

    /// <summary>
    /// Инициализирует и выбирает подходящий провайдер бэкапов
    /// </summary>
    private async Task<IBackupProvider> GetActiveProviderAsync()
    {
        if (_activeProvider != null)
        {
            return _activeProvider;
        }

        // Проверяем каждый провайдер по порядку приоритета
        foreach (var provider in _providers)
        {
            if (await provider.IsAvailableAsync())
            {
                _activeProvider = provider;
                _logger.LogInformation("🔧 Выбран провайдер бэкапов: {Provider} - {Description}",
                    provider.ProviderType, provider.Description);
                return _activeProvider;
            }
        }

        throw new InvalidOperationException("Ни один провайдер бэкапов не доступен в текущем окружении");
    }

    public async Task<string> CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        var provider = await GetActiveProviderAsync();
        var backupPath = await provider.CreateBackupAsync(cancellationToken);

        // Удаляем старые бекапы после успешного создания
        await CleanupOldBackupsAsync();

        return backupPath;
    }

    public async Task<bool> RestoreBackupAsync(string backupFilePath, CancellationToken cancellationToken = default)
    {
        var provider = await GetActiveProviderAsync();
        return await provider.RestoreBackupAsync(backupFilePath, cancellationToken);
    }

    public async Task<List<BackupInfo>> GetBackupListAsync()
    {
        // Получаем список бэкапов от всех доступных провайдеров
        var allBackups = new List<BackupInfo>();

        foreach (var provider in _providers)
        {
            try
            {
                if (await provider.IsAvailableAsync())
                {
                    var backups = await provider.GetBackupListAsync();
                    allBackups.AddRange(backups);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Ошибка при получении списка бекапов от провайдера {Provider}", provider.ProviderType);
            }
        }

        return allBackups.OrderByDescending(b => b.CreatedAt).ToList();
    }

    public async Task<bool> DeleteBackupAsync(string backupFilePath)
    {
        var provider = await GetActiveProviderAsync();
        return await provider.DeleteBackupAsync(backupFilePath);
    }

    private async Task CleanupOldBackupsAsync()
    {
        try
        {
            var backups = await GetBackupListAsync();

            if (backups.Count > _options.MaxBackupsToKeep)
            {
                var backupsToDelete = backups.Skip(_options.MaxBackupsToKeep).ToList();

                foreach (var backup in backupsToDelete)
                {
                    await DeleteBackupAsync(backup.FilePath);
                }

                _logger.LogInformation("🧹 Удалено старых бекапов: {Count}", backupsToDelete.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Ошибка при очистке старых бекапов");
        }
    }
}

public class BackupInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public double SizeMb { get; set; }
    public string ProviderType { get; set; } = string.Empty;
}
