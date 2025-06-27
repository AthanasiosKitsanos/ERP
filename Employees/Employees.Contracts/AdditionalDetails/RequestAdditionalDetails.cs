using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.AdditionalDetailsContract;

public class RequestAdditionalDetails
{
    public class Create
    {
        public int EmployeeId { get; set; }
        public string EmergencyNumbers { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public IFormFile? CertificationFile { get; set; }

        public IFormFile? PersonalDocumentsFile { get; set; }
    }

    public class Update
    {
        public int EmployeeId { get; set; }
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
    }
}
