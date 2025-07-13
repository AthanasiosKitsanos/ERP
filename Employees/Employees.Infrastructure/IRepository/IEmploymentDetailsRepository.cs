using Employees.Domain.Models;

namespace Employees.Infrastructure.Repository;

public interface IEmploymentDetailsRepository
{
    Task<EmploymentDetails> GetByIdAsync(int id, CancellationToken token = default);
    Task<bool> CreateAsync(EmploymentDetails details, CancellationToken token = default);
    Task<bool> UpdateAsync(EmploymentDetails details, CancellationToken token = default);
}
