using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.Payments;

public class PayGrades
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string GradeName { get; set; } = string.Empty;

    public string MinSalary { get; set; } = string.Empty;

    public string MaxSalary { get; set; } = string.Empty;

    public string PayGradePerNameId { get; set; } = string.Empty;

    [ForeignKey(nameof(PayGradePerNameId))]
    public PayGradePerName PayGradePerName { get; set; } = null!;
}
