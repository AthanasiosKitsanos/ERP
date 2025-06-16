namespace Employees.Domain.Models;

public class RolesList
{
    public int EmployeeId { get; set; }
    public int SelectedRoleId { get; set; }

    public List<Roles> List { get; set; } = new List<Roles>();
}

public class Roles
{
    public int Id { get; set; }

    public string RoleName { get; set; } = string.Empty;
}