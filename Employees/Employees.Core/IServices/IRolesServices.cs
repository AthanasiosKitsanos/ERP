using Employees.Contracts.RolesContract;

namespace Employees.Core.Services;

public interface IRolesServices
{
    IAsyncEnumerable<ResponseRoles.Get> GetAllAsync(CancellationToken token = default);
    Task<ResponseRoles.Get> GetRoleById(int id, CancellationToken token = default);

    Task<bool> CreateAsync(int id, RequestRoles.Create create, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, RequestRoles.Update update, CancellationToken token = default);
}
