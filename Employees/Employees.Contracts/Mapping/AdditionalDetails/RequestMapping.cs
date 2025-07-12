using System.Threading.Tasks;
using Employees.Contracts.AdDetails;
using Employees.Domain.Models;
using Employees.Shared;

namespace Employees.Contracts.Mapping.AdDetails;

public static class RequestMapping
{
    public static async Task<AdditionalDetails> MapToCreateRequest(this RequestAdditionDetails.Create create, int id)
    {
        return new AdditionalDetails
        {
            EmergencyNumbers = create.EmergencyNumbers,
            Education = create.Education,
            Certifications = await create.Certifications.GetArrayOfBytes(),
            CertMime = create.Certifications.ContentType,
            PersonalDocuments = await create.PersonalDocuments.GetArrayOfBytes(),
            DocMime = create.PersonalDocuments.ContentType,
            EmployeeId = id
        };
    }

    public static AdditionalDetails MapToUpdateRequest(this RequestAdditionDetails.Update update, int id)
    {
        return new AdditionalDetails
        {
            EmergencyNumbers = update.EmergencyNumbers,
            Education = update.Education,
            EmployeeId = id
        };
    }
}
