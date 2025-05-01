namespace ErpProject.Models;

public class AdditionalDetails
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public byte[] Identifications { get; set; } = new byte[0];
     public IFormFile? IdentificationFile { get; set; }

    public byte[] PersonalDocuments { get; set; } = new byte[0];
    public IFormFile? PersonalDocumentsFile { get; set; }

    public string MIME { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}