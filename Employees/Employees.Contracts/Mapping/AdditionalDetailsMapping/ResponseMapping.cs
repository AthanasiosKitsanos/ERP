using Employees.Contracts.AdditionalDetailsContract;
using Employees.Domain.Models;

namespace Employees.Contracts.AdditionalDetailsMapping;

public static class ResponseMapping
{
    public static ResponseAdditionalDetails.Get MapToGetResponse(this AdditionalDetails details)
    {
        return new ResponseAdditionalDetails.Get
        {
            EmergencyNumbers = details.EmergencyNumbers,
            Education = details.Education
        };
    }    
}
