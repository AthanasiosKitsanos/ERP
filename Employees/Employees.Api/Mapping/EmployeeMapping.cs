using Employees.Contracts;
using Employees.Domain.Models;
using Employees.Shared;

namespace Employees.Api.Mapping.Employees;

public static class EmployeeMapping
{
    public static ResponseEmployee MapToResponse(this Employee employee)
    {
        return new ResponseEmployee
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

    public static async Task<Employee> MapToCreateRequest(this RequestEmployee request)
    {
        return new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Age = request.Age,
            DateOfBirth = request.DateOfBirth,
            Nationality = request.Nationality,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Photograph = await request.PhotoFile.GetArrayOfBytes(),
            MIME = request.PhotoFile.ContentType
        };
    }
}
