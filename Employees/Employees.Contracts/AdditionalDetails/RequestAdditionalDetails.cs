using Microsoft.AspNetCore.Http;

namespace Employees.Contracts.AdditionalDetailsContract;

public class RequestAdditionalDetails
{
    public enum FormMode
    {
        View,
        Create,
        Update
    }

    public class Create
    {
        public int EmployeeId { get; set; }
        public string EmergencyNumbers { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public IFormFile? CertificationFile { get; set; }

        public IFormFile? PersonalDocumentsFile { get; set; }

        public FormMode Mode { get; set; }

        public string FormAction => Mode switch
        {
            FormMode.Create => "Create",
            FormMode.Update => "Update",
            _=> "View"
        };
    }

    public class Update
    {

        public int EmployeeId { get; set; }
        public string EmergencyNumbers { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
    }
}
