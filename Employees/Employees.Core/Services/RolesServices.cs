using System.Runtime.CompilerServices;
using Employees.Contracts.Mapping.RolesMapping;
using Employees.Contracts.RolesContract;
using Employees.Domain.Models;
using Employees.Infrastructure.Repository;

namespace Employees.Core.Services;

public class RolesServices : IRolesServices
{
    private readonly IRolesRepository _repository;

    public RolesServices(IRolesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(int id, RequestRoles.Create create, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        Roles role = create.MapToCreateRequest(id);

        return await _repository.CreateAsync(role, token);
    }

    public async IAsyncEnumerable<ResponseRoles.Get> GetAllAsync([EnumeratorCancellation] CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        await foreach (Roles role in _repository.GetAllAsync())
        {
            yield return role.MapToGetResponse();
        }
    }

    public async Task<ResponseRoles.Get> GetRoleById(int id, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        Roles role = await _repository.GetRoleById(id, token);

        return role.MapToGetResponse();
    }

    public async Task<bool> UpdateAsync(int id, RequestRoles.Update update, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        Roles role = update.MapToUpdateRequest(id);

        return await _repository.UpdateAsync(role, token);
    }
}
