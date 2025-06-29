using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.EmployeeContracts;

public class ResponseEmployee
{
    public class Create
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public required IFormFile PhotoFile { get; init; }
    }

    public class Get
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public byte[]? Photograph { get; set; }

        public string? MIME { get; set; }
    }

    public class Delete
    {
        public required int Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }

    public class Update
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
