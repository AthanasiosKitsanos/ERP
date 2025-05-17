using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class RolesServices
{
    private readonly Connection _connection;

    public RolesServices(Connection connection)
    {
        _connection = connection;
    }

    public async IAsyncEnumerable<Roles> GetRolesAsync()
    {
        string query = @"SELECT *
                        FROM Roles";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Roles role = new Roles
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
                        };

                        yield return role;
                    }
                }
            }
        }
    }

    public async Task<string> GetEmployeeRoleAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        string roleName = string.Empty;

        string query = @"SELECT RoleName
                        FROM Roles
                        JOIN RoleEmployee ON RoleEmployee.RoleId = Roles.Id
                        JOIN Employees ON Employees.EmployeeId = RoleEmployee.EmployeeId
                        WHERE Employees.EmployeeId = @EmployeeId";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        roleName = reader.GetString(reader.GetOrdinal("RoleName"));
                    }
                }
            }
        }

        return roleName;
    }
}
