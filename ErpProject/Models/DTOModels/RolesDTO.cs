using ErpProject.Models.RolesModel;

namespace ErpProject.Models.DTOModels;

public class RolesDTO
{
    public int SelectedRole { get; set; }
    public List<Roles> RolesNameList { get; set; } = new List<Roles>();
}