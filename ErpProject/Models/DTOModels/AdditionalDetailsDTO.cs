using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.DTOModels;

public class AdditionalDetailsDTO
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}