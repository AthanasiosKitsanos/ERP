using Employees.Domain.Models;
using Employees.Contracts.CredentialsContract;

namespace Employees.Contracts.CredentialsMapping;

public static class CredentialsMapping
{
    public static Credentials MapToCreateRequest(this RequestCredentials.Create request, int id)
    {
        return new Credentials
        {
            Username = request.Username,
            Password = request.Password,
            AccountStatus = request.AccountStatus,
            EmployeeId = id
        };
    }
}
