using Employees.Contracts.EmployeeContracts;
using Employees.Domain.Models;

namespace Employees.Contracts.EmployeesMapping;

public static class ResponseMapping
{
    public static ResponseEmployee.Get MapToGetResponse(this Employee employee)
    {
        if (employee is null)
        {
            return new ResponseEmployee.Get();
        }
        
        return new ResponseEmployee.Get
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Age = employee.Age,
            DateOfBirth = employee.DateOfBirth,
            Nationality = employee.Nationality,
            Gender = employee.Gender,
            PhoneNumber = employee.PhoneNumber,
            Photograph = employee.Photograph,
            MIME = employee.MIME
        };
    }

    public static ResponseEmployee.Delete MapToDeleteResponse(this Employee employee)
    {
        return new ResponseEmployee.Delete
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName
        };
    }
}
