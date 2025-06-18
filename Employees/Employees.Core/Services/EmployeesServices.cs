using Employees.Domain.Models;
using Employees.Core.IServices;
using Employees.Infrastructure.IRepository;
using System.Runtime.CompilerServices;

namespace Employees.Core.Services;

public class EmployeesServices: IEmployeesServices
{
    private readonly IEmployeesRepository _repository;

    public EmployeesServices(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CreateAsync(Employee employee)
    {
        await _repository.CreateAsync(employee);
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }
    public async IAsyncEnumerable<Employee> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (Employee employee in _repository.GetAllAsync(cancellationToken))
        {
            yield return employee;
        }
    }
    

    public Task<Employee> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, Employee employee)
    {
        throw new NotImplementedException();
    }
}
