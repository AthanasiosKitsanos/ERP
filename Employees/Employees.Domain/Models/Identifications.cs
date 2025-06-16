namespace Employees.Domain.Models;

public class Identifications
{
    public int Id;
    public string TIN { get; set; } = string.Empty;
    public string WorkAuth { get; set; } = string.Empty;
    public string TaxInformation { get; set; } = string.Empty;
    public int EmployeeId;
}
