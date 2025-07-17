using System.Collections;
using Employees.Domain.Models;

namespace Employees.Infrastructure.Repository;

public interface IRolesRepository
{
    IAsyncEnumerable<Roles> GetAllAsync(CancellationToken token = default);
    Task<Roles> GetRoleById(int id, CancellationToken token = default);

    Task<bool> CreateAsync(Roles role, CancellationToken token = default);

    Task<bool> UpdateAsync(Roles role, CancellationToken token = default);
}
