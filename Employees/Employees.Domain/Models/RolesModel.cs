namespace Employees.Domain.Models;

public class Roles
{
    public int Id { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}