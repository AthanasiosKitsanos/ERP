namespace ErpProject.Models;

public class EmploymentDetails
{
    public string Position { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string EmploymentStatus { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public string ContractType { get; set; } = string.Empty;

    public string WorkLocation { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}