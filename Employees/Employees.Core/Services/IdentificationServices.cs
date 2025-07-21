using Employees.Contracts.Identifications;
using Employees.Domain.Models;
using Employees.Infrastructure.Repository;

namespace Employees.Core.Services;

public class IdentificationServices: IIdentificationServices
{
    private readonly IIdentificationRepository _repository;

    public IdentificationServices(IIdentificationRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> CreateAsync(int id, RequestIdentifications.Create details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseIdentifications.Get> GetByIdAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, RequestIdentifications.Update details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
