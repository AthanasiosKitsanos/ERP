using System.Text.Json;

namespace ErpProject.JsonWebToken;

public class JWTDemoKey
{
    public string DemonstrationKey { get; set; } = string.Empty;

    public JWTDemoKey(IConfiguration configuration)
    {
        DemonstrationKey = configuration.GetSection("DemoKeySettings").GetValue<string>("DemoKey") ?? string.Empty;
    }

    public string Serialize() => JsonSerializer.Serialize(this);
}
