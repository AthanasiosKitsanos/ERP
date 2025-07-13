using Employees.Contracts.File;

namespace Employees.Core.Services;

public interface IFileServices
{
    Task<ResponseFile.GetPhoto> GetPhotoAsync(int id, CancellationToken token = default);
}
