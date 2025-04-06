using System;

namespace ErpProject.Models.DTOModels.Update;

public class UpdateDTO
{
    public int Id { get; set; } = 0;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
}