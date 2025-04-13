using System;
using System.ComponentModel.DataAnnotations.Schema;
using ErpProject.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;

namespace ErpProject.Models.PersonalDocumentsModels;

[Keyless]
public class PersonalDocuments
{
    public string DocumentsPath { get; set; } = string.Empty;

    public int EmployeeId { get ; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
}
