using Employees.Contracts.AdDetails;

namespace Employees.Core.Services;

public interface IAdditionalDetailsServices
{
    Task<bool> CreateAsync(int id, RequestAdDetails.Create details, CancellationToken token = default);
    Task<bool> UpdateAsync(int id, RequestAdDetails.Update details, CancellationToken token = default);
    Task<ResponseAdDetails.Get> GetAsync(int id, CancellationToken token = default);
}
