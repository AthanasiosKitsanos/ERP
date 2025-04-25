namespace ErpProject.Models;

public class Identifications
{
    public string TIN { get; set; } = string.Empty;

    public bool WorkAuth { get; set; }

    public string TaxInformation { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}