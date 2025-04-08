using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.DTOModels;

public class AdditionalDetailsDTO
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    [MaxLength(8000)]
    public byte[] Certifications { get; set; } = null!;

    [MaxLength(8000)]
    public byte[] PersonalDocuments { get; set; } = null!;

    public int EmployeeId { get; set; }
}
