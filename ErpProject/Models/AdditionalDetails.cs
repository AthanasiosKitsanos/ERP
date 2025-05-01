namespace ErpProject.Models;

public class AdditionalDetails
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public IFormFile? CertificationFile { get; set; }

    public IFormFile? PersonalDocumentsFile { get; set; }

    public int EmployeeId { get; set; }
}