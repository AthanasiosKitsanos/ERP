namespace ErpProject.Models;

public class Roles
{
    public int SelectedRole { get; set; }
    public List<Roles> RolesNameList { get; set; } = new List<Roles>();
}