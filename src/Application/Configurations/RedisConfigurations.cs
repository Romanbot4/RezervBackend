namespace Application.Configurations;

public sealed class RedisConfigurations
{
    public const string SettingKey = "Redis";

    public RedisConfigurations()
    {
        ConnectionString = string.Empty;
    }

    public string ConnectionString { get; set; }
}
