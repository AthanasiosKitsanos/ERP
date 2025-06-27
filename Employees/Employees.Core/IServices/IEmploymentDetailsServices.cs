using Employees.Contracts.EmploymentDetails;
using Employees.Domain.Models;

namespace Employees.Core.IServices;

public interface IEmploymentDetailsServices
{
    Task<bool> CreateAsync(int id, RequestEmploymentDetails.Create details, CancellationToken token = default);

    Task<bool> UpdateAsync(int id, RequestEmploymentDetails.Update details, CancellationToken token = default);

    Task<ResponseEmploymentDetails.Get> GetByIdAsync(int id, CancellationToken token = default);
}
