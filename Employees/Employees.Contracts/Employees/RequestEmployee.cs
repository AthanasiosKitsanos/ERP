using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.EmployeeContracts;

public class RequestEmployee
{
    public class Create
    {
        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;

        public DateOnly DateOfBirth { get; init; }

        public string Nationality { get; init; } = string.Empty;

        public string Gender { get; init; } = string.Empty;

        public string PhoneNumber { get; init; } = string.Empty;

        public required IFormFile PhotoFile { get; init; }
    }

    public class Update
    {
        public string Email { get; set; } = string.Empty;      
        public string Nationality { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class Photo
    {
        public required IFormFile PhotoFile { get; init; }
    }
}