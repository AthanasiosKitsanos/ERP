using Employees.Contracts.AdDetails;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.AdDetails;

public static class ResponseMapping
{
    public static ResponseAdDetails.Get MapToGetResponse(this AdditionalDetails details)
    {
        return new ResponseAdDetails.Get
        {
            EmergencyNumbers = details.EmergencyNumbers,
            Education = details.Education
        };
    }
}
