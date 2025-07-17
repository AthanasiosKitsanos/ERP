using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.AdDetails;

public class RequestAdditionDetails
{
    public class Create
    { 
        [Required(ErrorMessage = "Required")]
        public string EmergencyNumbers { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Education { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public IFormFile Certifications { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        public IFormFile PersonalDocuments { get; set; } = null!;
    }

    public class Update
    {
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
    }
}
