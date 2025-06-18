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

    public async Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _repository.CreateAsync(employee, cancellationToken);  
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _repository.DeleteByIdAsync(id, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _repository.EmailExistsAsync(email, cancellationToken);
    }

    public async IAsyncEnumerable<Employee> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await foreach (Employee employee in _repository.GetAllAsync(cancellationToken))
        {
            yield return employee;
        }
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> UpdateAsync(int id, Employee employee, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        return await _repository.UpdateAsync(id, employee, cancellationToken); 
    }
}
