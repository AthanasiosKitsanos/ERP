using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;

namespace Employees.Infrastructure.Repository;

public class AdditionalDetailsRepository : IAdditionalDetailsRepository
{
    private readonly Connection _connection;

    public AdditionalDetailsRepository(Connection connection)
    {
        _connection = connection;
    }

    public Task<bool> CreateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Credentials> GetAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
