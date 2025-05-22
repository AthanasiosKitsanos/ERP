using System.Text.Json;

namespace ErpProject.JsonWebToken;

public class JWTHeader
{
    public string algorithm { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;

    public JWTHeader(IConfiguration configuration)
    {
        IConfiguration section = configuration.GetSection("HeaderSetting");
        algorithm = section.GetValue<string>("algorithm") ?? string.Empty;
        type = section.GetValue<string>("type") ?? string.Empty;
    }

    public string Serialize() => JsonSerializer.Serialize(this);
}
