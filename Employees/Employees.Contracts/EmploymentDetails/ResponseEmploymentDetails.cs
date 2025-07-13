namespace Employees.Contracts.EmpDe;

public class ResponseEmploymentDetails
{
    public class Get
    {
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmploymentStatus { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public string ContractType { get; set; } = string.Empty;
        public string WorkLocation { get; set; } = string.Empty;
    }
}
