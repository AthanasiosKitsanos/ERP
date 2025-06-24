using Employees.Contracts.EmployeeContracts;

namespace Employees.Core.IServices;

public interface IEmployeesServices
{
    Task<bool> EmailExistsAsync(string email, CancellationToken token = default);

    Task<int> CreateAsync(RequestEmployee.Create request, CancellationToken token = default);

    IAsyncEnumerable<ResponseEmployee.Get> GetAllAsync(CancellationToken token = default);

    Task<ResponseEmployee.Get> GetByIdAsync(int id, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, RequestEmployee.Update request, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);

    Task<ResponseEmployee.Delete> GetInfoForDeleteAysnc(int id, CancellationToken token = default);
}
