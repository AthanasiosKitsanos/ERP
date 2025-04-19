using System;
using ErpProject.Helpers.Connection;

namespace ErpProject.Services.EmployeeCredentialsServices;

public class EmployeeCredentialsService
{
    private readonly Connection _connection;

    public EmployeeCredentialsService(Connection connection)
    {
        _connection = connection;
    }

    // TODO: Add methods to relate the credantials to the employee
}