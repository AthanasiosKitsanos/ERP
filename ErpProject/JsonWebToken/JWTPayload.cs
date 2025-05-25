using System.Text.Json;
using ErpProject.Models;

namespace ErpProject.JsonWebToken;

public class JWTPayload
{
    public string Issuer { get; set; } = "http://localhost:5099";
    public string Audience { get; set; } = "http://localhost:5099";
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public long TokenValidityOnSeconds { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public long Expiration { get; set; }

    public string Serialize() => JsonSerializer.Serialize(this);
}