using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Reqiured!")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required!")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required!")]
    [EmailAddress(ErrorMessage = "Please etner a valid email address")]
    public string Email { get; set; } = string.Empty;

    public string Age { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required!")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Required!")]
    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required!")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required!")]
    public string PhoneNumber { get; set; } = string.Empty;

    public byte[] Photograph { get; set; } = new byte[0];

    public string MIME { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please upload an image.")]
    public IFormFile? PhotoFile { get; set; }
}