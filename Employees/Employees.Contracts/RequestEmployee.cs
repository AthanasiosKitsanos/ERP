using Microsoft.AspNetCore.Http;

namespace Employees.Contracts;

public class RequestEmployee
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Age { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public required IFormFile PhotoFile { get; set; }
}
