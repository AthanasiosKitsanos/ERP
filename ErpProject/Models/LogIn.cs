namespace ErpProject.Models;

public class LogIn
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
