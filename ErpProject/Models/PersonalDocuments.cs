namespace ErpProject.Models;

public class PersonalDocument
{
    public List<string> DocumentsPaths { get; set; } = new List<string>();

    public int EmployeeId { get ; set; }

    public List<string> FullPathList { get; set; } = new List<string>();
}
