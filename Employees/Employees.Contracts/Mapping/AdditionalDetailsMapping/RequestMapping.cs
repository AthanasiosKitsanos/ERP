using System.Threading.Tasks;
using Azure;
using Employees.Contracts.AdditionalDetailsContract;
using Employees.Domain.Models;
using Employees.Shared;

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

    public static async Task<AdditionalDetails> MapToCreate(this RequestAdditionalDetails.Create create)
    {
        return new AdditionalDetails
        {
            EmergencyNumbers = create.EmergencyNumbers,
            Education = create.Education,
            Certifications = await create.CertificationFile!.GetArrayOfBytes(),
            PersonalDocuments = await create.PersonalDocumentsFile!.GetArrayOfBytes(),
            CertMime = create.CertificationFile!.ContentType,
            DocMime = create.PersonalDocumentsFile!.ContentType
        };
    }
}
