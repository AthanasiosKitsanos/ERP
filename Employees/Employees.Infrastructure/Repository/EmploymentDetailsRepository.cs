using System;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.Extensions.Logging;

namespace Employees.Infrastructure.Repository;

public class EmploymentDetailsRepository : IEmploymentDetailsRepository
{
    private readonly ILogger<EmploymentDetailsRepository> _logger;

    public EmploymentDetailsRepository(ILogger<EmploymentDetailsRepository> logger)
    {
        _logger = logger;
    }
    
    public Task<bool> CreateAsync(int id, EmploymentDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<EmploymentDetails> GetByIdAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, EmploymentDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
