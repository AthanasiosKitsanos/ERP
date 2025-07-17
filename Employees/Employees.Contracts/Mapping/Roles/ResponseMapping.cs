using Employees.Contracts.RolesContract;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.RolesMapping;

public static class ResponseMapping
{
    public static ResponseRoles.Get MapToGetResponse(this Roles role)
    {
        return new ResponseRoles.Get
        {
            Id = role.Id,
            RoleName = role.RoleName
        };
    }
}
