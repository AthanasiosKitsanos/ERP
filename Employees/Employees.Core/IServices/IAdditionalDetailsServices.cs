using Employees.Contracts.AdditionalDetails;

namespace Employees.Core.Services;

public interface IAdditionalDetailsServices
{
    Task<bool> CreateAsync(RequestAdDetails.Create details, CancellationToken token = default);
    Task<bool> UpdateAsync(RequestAdDetails.Update details, CancellationToken token = default);
    Task<ResponseAdDetails.Get> GetAsync(int id, CancellationToken token = default);
}
