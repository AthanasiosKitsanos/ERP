using Employees.Contracts.AdditionalDetailsContract;
using Employees.Contracts.AdditionalDetailsMapping;
using Employees.Core.IServices;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.Extensions.Logging;

namespace Employees.Core.Services;

public class AdditionalDetailsServices : IAdditionalDetailsServices
{
    private readonly IAdditionalDetailsRepository _repository;
    private readonly ILogger<AdditionalDetailsServices> _logger;

    public AdditionalDetailsServices(IAdditionalDetailsRepository repository, ILogger<AdditionalDetailsServices> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(int id, RequestAdditionalDetails.Create createDetails, CancellationToken token = default)
    {
        //Need to update this
        AdditionalDetails details = await createDetails.MapToCreate(id);

        details.EmployeeId = id;

        _logger.LogInformation($"EmployeeId = {details.EmployeeId}");

        return await _repository.CreateAsync(details, token);
    }

    public async Task<ResponseAdditionalDetails.Get> GetAsync(int id, CancellationToken token = default)
    {
        AdditionalDetails additionalDetails = await _repository.GetAsync(id, token);

        ResponseAdditionalDetails.Get details = additionalDetails.MapToGetResponse();
        
        return details;
    }

    public async Task<bool> UpdateAsync(int id, RequestAdditionalDetails.Update updateRequest, CancellationToken token = default)
    {
        AdditionalDetails details = updateRequest.MapToUpdate(id);

        return await _repository.UpdateAsync(details, token);
    }
}
