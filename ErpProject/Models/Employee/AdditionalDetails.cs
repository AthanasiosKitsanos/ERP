using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpProject.Models.EmployeeModel;

namespace ErpProject.Models.AdditionalDetailsModel;

public class AdditionalDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public string CertificationsPath { get; set; } = null!;

    public string PersonalDocumentsPath { get; set; } = null!;

    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
