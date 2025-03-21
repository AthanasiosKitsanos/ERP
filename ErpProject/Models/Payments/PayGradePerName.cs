using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Models.Payments;

public class PayGradePerName
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string GradeName { get; set; } = string.Empty;

    public string MinSalary { get; set; } = string.Empty;

    public string MaxSalary { get; set; } = string.Empty;
}
