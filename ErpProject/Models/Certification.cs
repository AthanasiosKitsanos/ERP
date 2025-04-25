namespace ErpProject.Models;

public class Certification
{
    public List<string> CertificationPaths { get; set; } = new List<string>();
    public int EmployeeId { get; set; }

    public List<string> FullPathList { get; set; } = new List<string>();
}
