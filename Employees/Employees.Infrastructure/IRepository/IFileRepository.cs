using Employees.Domain.Models;

namespace Employees.Infrastructure.IRepository;

public interface IFileRepository
{
    Task<Photo> GetPhotoAsync(int id, CancellationToken token = default);
}
