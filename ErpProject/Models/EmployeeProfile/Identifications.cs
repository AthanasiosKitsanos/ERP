using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace ErpProject.Models.EmployeeProfile;

public class Identifications
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string TIN { get; set; } = string.Empty;

    public bool WorkAuth { get; set; }

    public string TaxInformation { get; set; } = string.Empty;

    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }= null!;
}
