using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace ErpProject.Models.EmployeeProfile;

public class Identifications
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TIN { get; set; } = string.Empty;

    public bool WorkAuth { get; set; }

    public string TaxInformation { get; set; } = string.Empty;

    public string EmployeeId { get; set; } = string.Empty;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }= null!;
}
