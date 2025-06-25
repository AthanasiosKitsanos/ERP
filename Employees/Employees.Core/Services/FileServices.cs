using Employees.Contracts.File;
using Employees.Contracts.Mapping.File;
using Employees.Core.IServices;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;

namespace Employees.Core.Services;

public class FileServices : IFileServices
{
    private readonly IFileRepository _repository;

    public FileServices(IFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseFile.GetPhoto> GetPhotoAsync(int id, CancellationToken token = default)
    {
        Photo repositoryPhoto = await _repository.GetPhotoAsync(id, token);

        ResponseFile.GetPhoto photo = repositoryPhoto.MapToGetResponse();

        return photo;
    }
}
