using Employees.Domain.Models;
using Employees.Core.IServices;
using Employees.Infrastructure.IRepository;

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

    public IAsyncEnumerable<Employee> GetAllAsync()
    {
        throw new NotImplementedException();
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
