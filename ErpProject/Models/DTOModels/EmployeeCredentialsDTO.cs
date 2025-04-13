namespace ErpProject.Models.DTOModels;

public class EmployeeCredentialsDTO
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int AccountStatusId { get; set; }

    public int EmployeeId { get; set; }
}