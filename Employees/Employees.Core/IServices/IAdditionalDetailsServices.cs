using Employees.Contracts.AdDetails;

namespace Employees.Core.Services;

public interface IAdditionalDetailsServices
{
    Task<bool> CreateAsync(int id, RequestAdditionDetails.Create details, CancellationToken token = default);
    Task<bool> UpdateAsync(int id, RequestAdditionDetails.Update details, CancellationToken token = default);
    Task<ResponseAdditionalDetails.Get> GetAsyncById(int id, CancellationToken token = default);
}
