using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.DTOModels.Employee;

public class UpdateDTO
{
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(13, MinimumLength = 10)]
    public string? PhoneNumber { get; set; }
}
