namespace Application.Configurations;

public sealed class HangfireConfigurations
{
    public const string SettingKey = "Hangfire";

    public HangfireConfigurations()
    {
        Cron = "*/5 * * * *";
        BatchSize = 100;
        DashboardPath = "/hangfire";
        TablesPrefix = "hangfire_";
        WorkerCount = 2;
        QueuePollIntervalInSeconds = 15;
    }

    public string Cron { get; set; }

    public int BatchSize { get; set; }

    public string DashboardPath { get; set; }

    public string TablesPrefix { get; set; }

    public int WorkerCount { get; set; }

    public int QueuePollIntervalInSeconds { get; set; }
}
