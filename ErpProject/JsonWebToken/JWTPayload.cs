using System.Text.Json;
using ErpProject.Models;

namespace ErpProject.JsonWebToken;

public class JWTPayload
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public long TokenValidityOnSeconds { get; set; }
    public long Expiration { get; set; }

    public JWTPayload(LoggedInData data, bool rememberMe)
    {
        Issuer = "http://localhost:5099";
        Audience = "http://localhost:5099";
        Id = data.Id;
        FirstName = data.FirstName;
        LastName = data.LastName;
        Role = data.RoleName;
        TokenValidityOnSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Expiration = rememberMe ? DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds() : DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
    }

    public string Serialize() => JsonSerializer.Serialize(this);
}