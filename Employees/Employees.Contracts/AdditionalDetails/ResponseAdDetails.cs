namespace Employees.Contracts.AdDetails;

public class ResponseAdditionalDetails
{
    public class Get
    {
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
    }
}
