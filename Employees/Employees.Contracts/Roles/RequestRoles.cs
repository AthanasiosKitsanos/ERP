using System.ComponentModel.DataAnnotations;

namespace Employees.Contracts.RolesContract;

public class RequestRoles
{
    public class Create
    {
        [Required(ErrorMessage = "Required")]
        public int? RoleId { get; set; }
    }

    public class Update
    {
        public int? RoleId { get; set; }
    }
}
