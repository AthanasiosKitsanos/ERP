using System;

namespace ErpProject.Models.EmployeeProfile;

public class Roles
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string RoleName { get; set; } = string.Empty;
}
