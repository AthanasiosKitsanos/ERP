using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.EmployeeProfile;

public class RoleEpmloyee
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string RoleId { get; set; } = string.Empty;

    [ForeignKey(nameof(RoleId))]
    public Roles Role { get; set; } = null!;

    public string EmployeeId { get; set; } = string.Empty;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
