namespace ErpProject.Models.DTOModels;

public class ViewModelDTO
{
    public EmployeeDTO Employee { get; set;} = new EmployeeDTO();
    public AccountStatusDTO AccountStatus { get; set; } = new AccountStatusDTO();
    public AdditionalDetailsDTO AdditionalDetails { get; set;} = new AdditionalDetailsDTO();
    public EmployeeCredentialsDTO EmployeeCredential { get; set; } = new EmployeeCredentialsDTO();
    public EmploymentDetailsDTO EmploymentDetail { get; set; } =  new EmploymentDetailsDTO();
    public IdentificationsDTO Identifications { get; set; } = new IdentificationsDTO();
    public RolesDTO Roles { get; set; } = new RolesDTO();
}