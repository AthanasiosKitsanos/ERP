using System;

namespace Employees.Contracts.RolesContract;

public class ResponseRoles
{
    public class Get
    {
        public int? Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
