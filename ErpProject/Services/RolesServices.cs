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

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
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

    public async Task<Roles> GetEmployeeRoleAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        Roles role = new Roles();

        string query = @"SELECT *
                        FROM Roles
                        JOIN RoleEmployee ON RoleEmployee.RoleId = Roles.Id
                        WHERE RoleEmployee.EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        role.Id = reader.GetInt32(0);
                        role.RoleName = reader.GetString(1);
                    }
                }
            }
        }

        return role;
    }

    public async Task<bool> EditRoleAsync(int EmployeeId, int roleId)
    {
        if (EmployeeId <= 0 || roleId <= 0)
        {
            return false;
        }

        string query = @"UPDATE RoleEmployee
                        SET RoleId = @RoleId
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = EmployeeId;

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> AddRoleToEmployeeAsync(int employeeId, int roleId)
    {
        if (employeeId <= 0 || roleId <= 0)
        {
            return false;
        }

        string query = @"INSERT INTO RoleEmployee (RoleId, EmployeeId)
                        VALUES (@RoleId, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
