using System;
using System.ComponentModel.DataAnnotations;
using ErpProject.Models.EmployeeModel;

namespace ErpProject.Models.DTOModels;

public class EmployeeDTO
{
    public int Id { get; set; }

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
