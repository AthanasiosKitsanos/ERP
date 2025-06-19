using Employees.Domain.Models;

namespace Employees.Infrastructure.IRepository;

public interface IAdditionalDetailsRepository
{
    Task<bool> CreateAsync(AdditionalDetails details, CancellationToken token = default);

    Task<AdditionalDetails> GetAsync(int id, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, CancellationToken token = default);
}
