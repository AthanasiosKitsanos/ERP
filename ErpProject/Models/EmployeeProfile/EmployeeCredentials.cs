using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.EmployeeProfile;

public class EmployeeCredentials
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public DateTime LastLogIn { get; set; }

    public string AccountStatusId { get; set; } = string.Empty;

    [ForeignKey(nameof(AccountStatusId))]
    public AccountStatus AccountStatus { get; set; } = null!;

    public string EmployeeId { get; set; } = string.Empty;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
