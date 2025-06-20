using Employees.Contracts.Employee;
using Employees.Domain.Models;
using Employees.Shared;

namespace Employees.Api.Mapping.Employees;

public static class RequestMapping
{
    public static async Task<Employee> MapToCreateEmployee(this RequestEmployee.Create request)
    {
        return new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Age = request.DateOfBirth.CalculateAge(),
            DateOfBirth = request.DateOfBirth,
            Nationality = request.Nationality,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Photograph = await request.PhotoFile.GetArrayOfBytes(),
            MIME = request.PhotoFile.ContentType
        };
    }

    public static RequestEmployee.Update MapToUpdateRequest(this Employee employee)
    {
        return new RequestEmployee.Update
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Age = employee.Age,
            DateOfBirth = employee.DateOfBirth,
            Gender = employee.Gender
        };
    }

    public static Employee MapToUpdateEmployee(this RequestEmployee.Update request)
    {
        return new Employee
        {
            Id = request.Id,
            Email = request.Email,
            Nationality = request.Nationality,
            PhoneNumber = request.PhoneNumber
        };
    }
}
