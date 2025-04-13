using System;

namespace ErpProject.Models.DTOModels;

public class CertificationDTO
{
    public List<string> CertificationPaths { get; set; } = new List<string>();
    public int EmployeeId { get; set; }
}
