using System.Runtime.CompilerServices;
using Employees.Domain.Models;

namespace Employees.Core.IServices;

public interface IEmployeesServices
{
    Task<bool> EmailExistsAsync(string email);
    
    Task<int> CreateAsync(Employee employee);

    IAsyncEnumerable<Employee> GetAllAsync(CancellationToken token);

    Task<Employee> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, Employee employee);

    Task<bool> DeleteByIdAsync(int id);
}
