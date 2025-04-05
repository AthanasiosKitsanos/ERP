using System;
using ErpProject.Models.RolesModel;

namespace ErpProject.Models.PartialModels.AddRoleViewModel;

public class AddRoleViewModel
{
    public int EmployeeId { get; set; }
    public List<Roles> Roles { get; set; } = new List<Roles>();

    public string SelectedRole { get ; set; } = string.Empty; 
}
