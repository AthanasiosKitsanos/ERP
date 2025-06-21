using Employees.Domain.Models;
using Employees.Core.IServices;
using Employees.Infrastructure.IRepository;
using System.Runtime.CompilerServices;
using Employees.Core.QueryBuilders;
using Microsoft.Data.SqlClient;
using Employees.Shared.Validators;
using Employees.Contracts.EmployeeContracts;
using Employees.Contracts.EmployeesMapping;
using Employees.Api.Mapping.Employees;

namespace Employees.Core.Services;

public class EmployeesServices : IEmployeesServices
{
    private readonly IEmployeesRepository _repository;

    public EmployeesServices(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CreateAsync(RequestEmployee.Create request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Employee employee = await request.MapToCreateEmployee();

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

    public async IAsyncEnumerable<ResponseEmployee.Get> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await foreach (Employee employee in _repository.GetAllAsync(cancellationToken))
        {
            yield return employee.MapToGetResponse();;
        }
    }

    public async Task<ResponseEmployee.Get> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Employee employee = await _repository.GetByIdAsync(id);

        return employee.MapToGetResponse();
    }

    public async Task<bool> UpdateAsync(int id, ResponseEmployee.Update updateResponse, CancellationToken cancellationToken = default)
    {
        RequestEmployee.Update updateRequest = updateResponse.MapResponseToRequestUpdate();

        (string query, List<SqlParameter> sqlParameters) objects = updateRequest.UpdateDetailsQueryBuilder(id);

        if (objects.IsEmptyOrNull())
        {
            return false;
        }

        cancellationToken.ThrowIfCancellationRequested();

        return await _repository.UpdateAsync(objects.query, objects.sqlParameters, cancellationToken);
    }

    public async Task<ResponseEmployee.Delete> GetInfoForDeleteAysnc(int id, CancellationToken token = default)
    {
        Employee employee = await _repository.GetInfoForDeleteAysnc(id, token);

        return employee.MapToDeleteResponse();
    }
}
