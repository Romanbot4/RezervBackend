namespace Application.Configurations;

public sealed class JwtConfigurations
{
    public const string SettingKey = "Jwt";

    public JwtConfigurations()
    {
        AccessTokenKey = string.Empty;
        RefreshTokenKey = string.Empty;
        AccesTokenLifeSpanInMinutes = 0;
        RefreshTokenLifeSpanInMinutes = 0;
    }

    public string AccessTokenKey { get; set; }
    public string RefreshTokenKey { get; set; }

    public int AccesTokenLifeSpanInMinutes { get; set; }
    public int RefreshTokenLifeSpanInMinutes { get; set; }
}
