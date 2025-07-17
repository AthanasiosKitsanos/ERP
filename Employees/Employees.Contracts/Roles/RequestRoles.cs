namespace Employees.Contracts.RolesContract;

public class RequestRoles
{
    public class Create
    {
        public int RoleId { get; set; }
    }

    public class Update
    {
        public int RoleId { get; set; }
    }
}
