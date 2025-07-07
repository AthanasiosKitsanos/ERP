using Employees.Contracts.AdDetails;
using Employees.Contracts.Mapping.AdDetails;
using Employees.Domain.Models;
using Employees.Infrastructure.Repository;

namespace Employees.Core.Services;

public class AdditionalDetailsServices : IAdditionalDetailsServices
{
    private readonly IAdditionalDetailsRepository _repository;

    public AdditionalDetailsServices(IAdditionalDetailsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(int id, RequestAdDetails.Create create, CancellationToken token = default)
    {
        AdditionalDetails details = await create.MapToCreateRequest(id);

        return await _repository.CreateAsync(details, token);
    }

    public async Task<ResponseAdDetails.Get> GetAsync(int id, CancellationToken token = default)
    {
        AdditionalDetails details = await _repository.GetAsync(id, token);

        return details.MapToGetResponse();
    }

    public async Task<bool> UpdateAsync(int id, RequestAdDetails.Update update, CancellationToken token = default)
    {
        AdditionalDetails details = update.MapToUpdateRequest(id);

        return await _repository.UpdateAsync(details, token);
    }
}
