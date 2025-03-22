using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpProject.Models.Payments;

public class PayGrades
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string GradeName { get; set; } = string.Empty;

    public string MinSalary { get; set; } = string.Empty;

    public string MaxSalary { get; set; } = string.Empty;

    public int PayGradePerNameId { get; set; }

    [ForeignKey(nameof(PayGradePerNameId))]
    public PayGradePerName PayGradePerName { get; set; } = null!;
}
