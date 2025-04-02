using System;
using ErpProject.Models.EmployeeProfile;

namespace ErpProject.Models.PartialModels.AddRoleViewModel;

public class AddRoleViewModel
{
    public int EmployeeId { get; set; }
    public List<Roles>? Roles { get; set; }

    public string SelectedRole { get ; set; } = string.Empty; 
}
