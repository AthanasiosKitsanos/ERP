using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpProject.Models.AccountStatusModel;
using ErpProject.Models.EmployeeModel;

namespace ErpProject.Models.EmployeeCredentilasModel;

public class EmployeeCredentials
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public DateTime LastLogIn { get; set; }

    public int AccountStatusId { get; set; }

    [ForeignKey(nameof(AccountStatusId))]
    public AccountStatus AccountStatus { get; set; } = null!;

    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
