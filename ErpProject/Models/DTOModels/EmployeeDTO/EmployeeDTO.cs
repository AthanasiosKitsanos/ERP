using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.DTOModels.EmployeeDTO;

public class EmployeeDTO
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Age { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
    public DateTime DateOfBirth { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PhotographPath { get; set; } = string.Empty;
}
