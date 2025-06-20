using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.Employee;

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
        public int Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Age { get; init; } = string.Empty;
        public DateOnly DateOfBirth { get; init; }
        public string Nationality { get; init; } = string.Empty;
        public string Gender { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }

    public class Photo
    {
        public required IFormFile PhotoFile { get; init; }
    }
}