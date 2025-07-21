using Employees.Contracts.Identifications;

namespace Employees.Core.Services;

public interface IIdentificationServices
{
    Task<ResponseIdentifications.Get> GetByIdAsync(int id, CancellationToken token = default);
    Task<bool> CreateAsync(int id, RequestIdentifications.Create details, CancellationToken token = default);
    Task<bool> UpdateAsync(int id, RequestIdentifications.Update details, CancellationToken token = default);
}
