using Microsoft.Extensions.Options;

namespace bryx_CRM.Services;

public class AutoBackupHostedService : BackgroundService
{
    private readonly ILogger<AutoBackupHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private Timer? _timer;

    public AutoBackupHostedService(
        ILogger<AutoBackupHostedService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var enabled = _configuration.GetValue<bool>("BackupOptions:AutoBackupEnabled");

        if (!enabled)
        {
            _logger.LogInformation("⏸️ Автоматические бекапы отключены в настройках");
            return Task.CompletedTask;
        }

        var intervalHours = _configuration.GetValue<int>("BackupOptions:AutoBackupIntervalHours");
        if (intervalHours <= 0)
        {
            intervalHours = 24; // По умолчанию раз в сутки
        }

        var interval = TimeSpan.FromHours(intervalHours);

        _logger.LogInformation("⏰ Автоматические бекапы запущены. Интервал: {Hours} часов", intervalHours);

        // Запускаем первый бекап через 1 минуту после старта
        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.FromMinutes(1),
            interval
        );

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            _logger.LogInformation("🔄 Запуск автоматического бекапа...");

            using var scope = _serviceProvider.CreateScope();
            var backupService = scope.ServiceProvider.GetRequiredService<BackupService>();

            var backupPath = await backupService.CreateBackupAsync();

            _logger.LogInformation("✅ Автоматический бекап завершён: {Path}", backupPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Ошибка при выполнении автоматического бекапа");
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("⏹️ Остановка службы автоматических бекапов");
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
