using Employees.Contracts.AdditionalDetails;

namespace Employees.Core.IServices;

public interface IAdditionalDetailsServices
{
    Task<bool> CreateAsync(RequestAdditionalDetails.Create createDetails, CancellationToken token = default);
    Task<bool> UpdateAsync(RequestAdditionalDetails.Update updateRequest, CancellationToken token = default);
    Task<ResponseAdditionalDetails.Get> GetAsync(int id, CancellationToken token = default);
}
