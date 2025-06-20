using Employees.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.IRepository;

public interface IEmployeesRepository
{
    Task<bool> EmailExistsAsync(string email, CancellationToken token = default);

    Task<int> CreateAsync(Employee employee, CancellationToken token = default);

    IAsyncEnumerable<Employee> GetAllAsync(CancellationToken token = default);

    Task<Employee> GetByIdAsync(int id, CancellationToken token = default);

    Task<bool> UpdateAsync(string query, List<SqlParameter> sqlParameters, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);

    Task<Employee> GetInfoForDeleteAysnc(int id, CancellationToken token = default);
}
