using System;
namespace ErpProject.Models;

public class Identifications
{
    public int Id;
    public string TIN { get; set; } = string.Empty;
    public bool WorkAuth { get; set; }
    public string TaxInformation { get; set; } = string.Empty;
    public int Employee_Id;
}
