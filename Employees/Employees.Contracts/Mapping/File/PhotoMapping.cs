using Employees.Contracts.File;
using Employees.Domain.Models;

namespace Employees.Contracts.Mapping.File;

public static class PhotoMapping
{
    public static ResponseFile.GetPhoto MapToGetResponse(this Photo photo)
    {
        return new ResponseFile.GetPhoto
        {
            Photograph = photo.Photograph,
            Mime = photo.MIME
        };
    }
}
