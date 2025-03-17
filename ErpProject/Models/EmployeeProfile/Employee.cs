using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.EmployeeProfile;

public class Employee
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Age { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PhotographPath { get; set; } = string.Empty;
}
