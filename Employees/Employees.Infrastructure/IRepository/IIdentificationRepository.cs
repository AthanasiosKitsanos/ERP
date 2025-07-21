using Employees.Domain.Models;

namespace Employees.Infrastructure.Repository;

public interface IIdentificationRepository
{
    Task<Identifications> GetByIdAsync(int id, CancellationToken token = default);
    Task<bool> CreateAsync(int id, Identifications details, CancellationToken token = default);
    Task<bool> UpdateAsync(int id, Identifications details, CancellationToken token = default);
}