using Employees.Contracts.Employee;
using Employees.Domain.Models;
using Employees.Shared;

namespace Employees.Api.Mapping.Employees;

public static class RequestMapping
{
    public static async Task<Employee> MapToCreateRequest(this RequestEmployee.Create request)
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
}
