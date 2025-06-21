namespace Employees.Domain.Models;

public class Credentials
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string AccountStatus { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}