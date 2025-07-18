using System;
using System.Data;
using System.Runtime.CompilerServices;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class RolesRepository : IRolesRepository
{
    private readonly Connection _connection;

    public RolesRepository(Connection connection)
    {
        _connection = connection;
    }
    public async Task<bool> CreateAsync(Roles role, CancellationToken token = default)
    {
        string query = @"INSERT INTO dbo.RoleEmployee (RoleId, EmployeeId)
                        VALUES (@RoleId, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RoleId", SqlDbType.Int).Value = role.Id;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = role.EmployeeId;

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }

    public async IAsyncEnumerable<Roles> GetAllAsync([EnumeratorCancellation] CancellationToken token = default)
    {
        string query = @"SELECT Id, RoleName
                        FROM Roles";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    while (await reader.ReadAsync(token))
                    {
                        yield return new Roles
                        {
                            Id = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        };
                    }
                }
            }
        }
    }

    public async Task<Roles> GetRoleById(int id, CancellationToken token = default)
    {
        string query = @"SELECT r.RoleName
                        FROM Roles AS r
                        JOIN RoleEmployee AS re ON re.RoleId = r.Id
                        WHERE re.EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (await reader.ReadAsync(token))
                    {
                        return new Roles
                        {
                            RoleName = reader.GetString(0)
                        };
                    }
                }
            }
        }

        return new Roles();
    }

    public async Task<bool> UpdateAsync(Roles role, CancellationToken token = default)
    {
        string query = @"UPDATE RoleEmployee
                        SET RoleId = @RoleId
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RoleId", SqlDbType.Int).Value = role.Id;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = role.EmployeeId;

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }
}
