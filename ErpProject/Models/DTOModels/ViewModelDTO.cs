using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.DTOModels;

public class ViewModelDTO
{
    public EmployeeDTO Employee { get; set;} = new EmployeeDTO();

    public AccountStatusDTO AccountStatus { get; set; } = new AccountStatusDTO();

    public AdditionalDetailsDTO AdditionalDetails { get; set;} = new AdditionalDetailsDTO();

    public EmployeeCredentialsDTO EmployeeCredential { get; set; } = new EmployeeCredentialsDTO();

    public EmploymentDetailsDTO EmploymentDetails { get; set; } =  new EmploymentDetailsDTO();

    public IdentificationsDTO Identifications { get; set; } = new IdentificationsDTO();

    public RolesDTO Roles { get; set; } = new RolesDTO();

    public CertificationDTO Certifications { get; set; } = new CertificationDTO();

    public PersonalDocumentsDTO PersonalDocuments { get; set; } = new PersonalDocumentsDTO();
    
    public IFormFile ProfilePhoto { get; set; } = null!;

    public List<IFormFile> CertificationPDF { get; set; } = new List<IFormFile>();

    public List<IFormFile> PersonalDocumentsPDF { get; set; } = new List<IFormFile>();
}