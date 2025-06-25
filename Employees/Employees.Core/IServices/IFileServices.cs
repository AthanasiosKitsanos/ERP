using Employees.Contracts.File;
using Employees.Domain.Models;

namespace Employees.Core.IServices;

public interface IFileServices
{
    Task<ResponseFile.GetPhoto> GetPhotoAsync(int id, CancellationToken token = default);
}
