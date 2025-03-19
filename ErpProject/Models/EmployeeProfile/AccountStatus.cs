using System;

namespace ErpProject.Models.EmployeeProfile;

public class AccountStatus
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string StatusName { get; set; } = string.Empty;
}
