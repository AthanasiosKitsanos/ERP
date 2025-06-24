using Employees.Contracts.EmployeeContracts;
using Employees.Domain.Models;
using Employees.Shared;

namespace Employees.Contracts.EmployeesMapping;

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

    public static Employee MapResponseToUpdateEmployee(this RequestEmployee.Update updateResponse, int id)
    {
        return new Employee
        {
            Id = id,
            Email = updateResponse.Email,
            Nationality = updateResponse.Nationality,
            PhoneNumber = updateResponse.PhoneNumber
        };
    }

    public static RequestEmployee.Update MapToUpdateRequest(this ResponseEmployee.Update response)
    {
        return new RequestEmployee.Update
        {
            Email = response.Email,
            Nationality = response.Nationality,
            PhoneNumber = response.PhoneNumber
        };
    }
}
