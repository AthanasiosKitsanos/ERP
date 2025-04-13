using System;

namespace ErpProject.Models.DTOModels;

public class PersonalDocumentsDTO
{
    public List<string> DocumentsPaths { get; set; } = new List<string>();

    public int EmployeeId { get ; set; }
}
