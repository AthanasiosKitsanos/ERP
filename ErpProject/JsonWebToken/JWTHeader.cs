using System.Text.Json;

namespace ErpProject.JsonWebToken;

public class JWTHeader
{
    public string algorithm { get; set; } = "HS256";
    public string type { get; set; } = "JWT";

    public string Serialize() => JsonSerializer.Serialize(this);
}
