using Employees.Domain.Models;

namespace Employees.Infrastructure.Repository;

public interface IFileRepository
{
    Task<Photo> GetPhotoAsync(int id, CancellationToken token = default);
}
