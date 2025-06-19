using System.Runtime.CompilerServices;
using Employees.Domain.Models;

namespace Employees.Core.IServices;

public interface IEmployeesServices
{
    Task<bool> EmailExistsAsync(string email, CancellationToken token = default);

    Task<int> CreateAsync(Employee employee, CancellationToken token = default);

    IAsyncEnumerable<Employee> GetAllAsync(CancellationToken token = default);

    Task<Employee> GetByIdAsync(int id, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, Employee employee, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);

    Task<Employee> GetInfoForDeleteAysnc(int id, CancellationToken token = default);
}
