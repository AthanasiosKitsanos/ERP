using Employees.Contracts.RolesContract;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.RolesMapping;

public static class RequestMapping
{
    public static Roles MapToCreateRequest(this RequestRoles.Create create, int id)
    {
        return new Roles
        {
            Id = create.Id,
            EmployeeId = id
        };
    }

    public static Roles MapToUpdateRequest(this RequestRoles.Update update, int id)
    {
        return new Roles
        {
            Id = update.Id,
            EmployeeId = id
        };
    }
}
