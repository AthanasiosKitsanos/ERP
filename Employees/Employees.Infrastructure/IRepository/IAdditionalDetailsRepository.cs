using Employees.Domain.Models;

namespace Employees.Infrastructure.Repository;

public interface IAdditionalDetailsRepository
{
    Task<bool> CreateAsync(AdditionalDetails details, CancellationToken token = default);
    Task<bool> UpdateAsync(AdditionalDetails details, CancellationToken token = default);
    Task<AdditionalDetails> GetAsyncById(int id, CancellationToken token = default);
}