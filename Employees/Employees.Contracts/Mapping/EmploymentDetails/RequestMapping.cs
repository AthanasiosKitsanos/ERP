using Employees.Contracts.EmpDe;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.EmpDe;

public static class RequestMapping
{
    public static EmploymentDetails MapToCreateRequest(this RequestEmploymentDetails.Create create, int id)
    {
        return new EmploymentDetails
        {
            Position = create.Position,
            Department = create.Department,
            EmploymentStatus = create.EmploymentStatus,
            HireDate = create.HireDate,
            ContractType = create.ContractType,
            WorkLocation = create.WorkLocation,
            EmployeeId = id
        };
    }

    public static EmploymentDetails MapToUpdateRequest(this RequestEmploymentDetails.Update update, int id)
    {
        return new EmploymentDetails
        {
            Position = update.Position,
            Department = update.Department,
            EmploymentStatus = update.EmploymentStatus,
            HireDate = update.HireDate,
            ContractType = update.ContractType,
            WorkLocation = update.WorkLocation,
            EmployeeId = id
        };
    }
}
