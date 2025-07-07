namespace Employees.Contracts.AdditionalDetails;

public class ResponseAdDetails
{
    public class Get
    {
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
    }
}
