using System;

namespace ErpProject.Models.DTOModels;

public class IdentificationsDTO
{
    public string TIN { get; set; } = string.Empty;

    public bool WorkAuth { get; set; }

    public string TaxInformation { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}
