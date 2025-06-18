using Employees.Domain.Models;

namespace Employees.Infrastructure.IRepository;

public interface ICredentialsRepository
{
    Task<bool> CreateAsync(Credentials credential, CancellationToken token = default);

    Task<bool> UsernameExistsAsync(string username, CancellationToken token);

    Task UpdatePasswordHashAsync(int id, string username, string password, CancellationToken token);
}
