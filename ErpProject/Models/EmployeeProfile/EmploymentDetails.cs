using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.EmployeeProfile;

public class EmploymentDetails
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Position { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string EmploymentStatus { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public string ContractType { get; set; } = string.Empty;

    public string WorkLocation { get; set; } = string.Empty;

    public string EmployeeId { get; set; } = string.Empty;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
