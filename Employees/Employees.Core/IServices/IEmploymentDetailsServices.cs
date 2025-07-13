using Employees.Contracts.EmpDe;

namespace Employees.Core.Services;

public interface IEmploymentDetailsServices
{
    Task<ResponseEmploymentDetails.Get> GetByIdAsync(int id, CancellationToken token = default);
    Task<bool> CreateAsync(int id, RequestEmploymentDetails.Create details, CancellationToken token = default);
    Task<bool> UpdateAsync(int id, RequestEmploymentDetails.Update details, CancellationToken token = default);
}
