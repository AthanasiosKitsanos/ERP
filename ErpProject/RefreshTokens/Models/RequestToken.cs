namespace ErpProject.RefreshTokens.Models;

public class RequestToken
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
