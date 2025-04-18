using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpProject.Models.RolesModel;
using ErpProject.Models.EmployeeModel;

namespace ErpProject.Models.RolesEmployeeModel;

public class RoleEpmloyee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Roles Role { get; set; } = null!;

    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
