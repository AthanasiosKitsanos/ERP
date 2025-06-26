using Microsoft.AspNetCore.Http;

namespace Employees.Domain.Models;

public class AdditionalDetails
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public byte[] Certifications{ get; set; } = null!;

    public byte[] PersonalDocuments { get; set; } = null!;

    public string CertMime { get; set; } = string.Empty;

    public string DocMime { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}