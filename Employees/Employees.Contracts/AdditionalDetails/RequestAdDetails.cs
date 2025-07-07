using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.AdditionalDetails;

public class RequestAdDetails
{
    public class Create
    {
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;

        public IFormFile Certifications { get; set; } = null!;

        public IFormFile PersonalDocuments { get; set; } = null!;
    }

    public class Update
    {
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
    }
}
