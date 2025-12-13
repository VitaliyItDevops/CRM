namespace bryx_CRM.Services;

/// <summary>
/// Интерфейс для провайдеров бэкапов
/// </summary>
public interface IBackupProvider
{
    /// <summary>
    /// Тип провайдера (для отображения в UI)
    /// </summary>
    string ProviderType { get; }

    /// <summary>
    /// Описание провайдера
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Проверяет, доступен ли провайдер в текущем окружении
    /// </summary>
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Создаёт новый бэкап базы данных
    /// </summary>
    Task<string> CreateBackupAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Восстанавливает базу данных из бэкапа
    /// </summary>
    Task<bool> RestoreBackupAsync(string backupFilePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает список всех доступных бэкапов
    /// </summary>
    Task<List<BackupInfo>> GetBackupListAsync();

    /// <summary>
    /// Удаляет указанный бэкап
    /// </summary>
    Task<bool> DeleteBackupAsync(string backupFilePath);
}
