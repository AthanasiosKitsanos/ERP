using Employees.Contracts.AdditionalDetails;
using Employees.Core.IServices;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;

namespace Employees.Core.Services;

public class AdditionalDetailsServices : IAdditionalDetailsServices
{
    private readonly IAdditionalDetailsRepository _repository;

    public AdditionalDetailsServices(IAdditionalDetailsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(RequestAdditionalDetails.Create createDetails, CancellationToken token = default)
    {
        //Need to update this
        AdditionalDetails details = new AdditionalDetails();

        return await _repository.CreateAsync(details, token);
    }

    public async Task<ResponseAdditionalDetails.Get> GetAsync(int id, CancellationToken token = default)
    {
        //Need to update this
        AdditionalDetails additionalDetails = await _repository.GetAsync(id, token);

        ResponseAdditionalDetails.Get details = new ResponseAdditionalDetails.Get();

        return details;
    }

    public Task<bool> UpdateAsync(RequestAdditionalDetails.Update updateRequest, CancellationToken token = default)
    {
        //Need to update this
        throw new NotImplementedException();
    }
}
