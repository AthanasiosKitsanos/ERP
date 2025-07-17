using System.ComponentModel.DataAnnotations;

namespace Employees.Contracts.EmpDe;

public class RequestEmploymentDetails
{
    public class Create
    {
        [Required(ErrorMessage = "Required")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string EmploymentStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ContractType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string WorkLocation { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
    }

    public class Update
    {
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmploymentStatus { get; set; } = string.Empty;
        public DateOnly? HireDate { get; set; } = null;
        public string ContractType { get; set; } = string.Empty;
        public string WorkLocation { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
    }
}
