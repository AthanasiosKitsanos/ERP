using Employees.Core.IServices;
using Employees.Infrastructure.IRepository;
using Microsoft.Extensions.Logging;
using Employees.Contracts.EmploymentDetails;

namespace Employees.Core.Services;

public class EmploymentDetailsServices : IEmploymentDetailsServices
{
    private readonly IEmploymentDetailsRepository _repository;
    private readonly ILogger<EmploymentDetailsServices> _logger;

    public EmploymentDetailsServices(IEmploymentDetailsRepository repository, ILogger<EmploymentDetailsServices> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public Task<bool> CreateAsync(int id, RequestEmploymentDetails.Create details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseEmploymentDetails.Get> GetByIdAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, RequestEmploymentDetails.Update details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
