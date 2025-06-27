using Employees.Domain.Models;

namespace Employees.Infrastructure.IRepository;

public interface IEmploymentDetailsRepository
{
    Task<bool> CreateAsync(int id, EmploymentDetails details, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, EmploymentDetails details, CancellationToken token = default);

    Task<EmploymentDetails> GetByIdAsync(int id, CancellationToken token = default);
}
