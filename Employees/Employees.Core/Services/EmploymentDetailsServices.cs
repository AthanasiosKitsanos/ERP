using Employees.Contracts.EmpDe;
using Employees.Contracts.Mapping.EmpDe;
using Employees.Domain.Models;
using Employees.Infrastructure.Repository;

namespace Employees.Core.Services;

public class EmploymentDetailsServices : IEmploymentDetailsServices
{
    private readonly IEmploymentDetailsRepository _repository;

    public EmploymentDetailsServices(IEmploymentDetailsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(int id,RequestEmploymentDetails.Create request, CancellationToken token = default)
    {
        EmploymentDetails details = request.MapToCreateRequest(id);

        return await _repository.CreateAsync(details, token);
    }

    public async Task<ResponseEmploymentDetails.Get> GetByIdAsync(int id, CancellationToken token = default)
    {
        EmploymentDetails details = await _repository.GetByIdAsync(id, token);

        return details.MapToGetResponse();
    }

    public async Task<bool> UpdateAsync(int id, RequestEmploymentDetails.Update request, CancellationToken token = default)
    {
        EmploymentDetails details = request.MapToUpdateRequest(id);

        return await _repository.UpdateAsync(details, token);
    }
}
