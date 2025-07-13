using Employees.Domain.Models;
using Employees.Infrastructure.Repository;

namespace Employees.Core.Services;

public class CredentialsServices : ICredentialsServices
{
    private readonly ICredentialsRepository _repository;

    public CredentialsServices(ICredentialsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(Credentials credential, CancellationToken token = default)
    {
        return await _repository.CreateAsync(credential, token);
    }

    public async Task UpdatePasswordHashAsync(int id, string username, string password, CancellationToken token)
    {
        await _repository.UpdatePasswordHashAsync(id, username, password, token);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken token)
    {
        return await _repository.UsernameExistsAsync(username, token);
    }
}
