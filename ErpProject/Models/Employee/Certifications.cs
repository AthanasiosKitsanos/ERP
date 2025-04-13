using ErpProject.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.CertificationModel;

[Keyless]
public class Certifications
{
    public string CertificationPath { get; set; } = string.Empty;
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
