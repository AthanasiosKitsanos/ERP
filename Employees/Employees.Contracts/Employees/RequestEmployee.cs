using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Employees.Contracts.EmployeeContracts;

public class RequestEmployee
{
    public class Create
    {
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string LastName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; init; }

        [Required(ErrorMessage = "Required")]
        public string Nationality { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Gender { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string PhoneNumber { get; init; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public required IFormFile PhotoFile { get; init; }
    }

    public class Update
    {
        public string Email { get; init; } = string.Empty;      
        public string Nationality { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }

    public class Photo
    {
        public required IFormFile PhotoFile { get; init; }
    }
}