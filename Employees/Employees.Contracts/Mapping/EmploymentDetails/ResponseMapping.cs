using Employees.Contracts.EmpDe;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.EmpDe;

public static class ResponseMapping
{
    public static ResponseEmploymentDetails.Get MapToGetResponse(this EmploymentDetails details)
    {
        return new ResponseEmploymentDetails.Get
        {
            Position = details.Position,
            Department = details.Department,
            EmploymentStatus = details.EmploymentStatus,
            HireDate = details.HireDate,
            ContractType = details.ContractType,
            WorkLocation = details.WorkLocation
        };
    }
}
