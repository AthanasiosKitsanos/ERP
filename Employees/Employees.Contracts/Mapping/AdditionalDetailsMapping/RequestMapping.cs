using Azure;
using Employees.Contracts.AdditionalDetailsContract;

namespace Employees.Contracts.AdditionalDetailsMapping;

public static class RequestMapping
{
    public static RequestAdditionalDetails.Update MapToUpdateRequest(this ResponseAdditionalDetails.Get details)
    {
        return new RequestAdditionalDetails.Update
        {
            EmergencyNumbers = details.EmergencyNumbers,
            Education = details.Education
        };
    }
}
