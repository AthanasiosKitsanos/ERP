using Employees.Contracts.Credentials;
using Employees.Domain.Models;

namespace Employees.Api.Mapping;

public static class CredentialsMapping
{
    public static Credentials MapToCreateRequest(this RequestCredentials.Create request)
    {
        return new Credentials
        {
            Username = request.Username,
            Password = request.Password,
            AccountStatus = request.AccountStatus,
            EmployeeId = request.EmployeeId
        };
    }
}
