using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class CredentialsRepository : ICredentialsRepository
{
    private readonly Connection _connection;

    public CredentialsRepository(Connection connection)
    {
        _connection = connection;
    }
    public async Task<bool> CreateAsync(Credentials credentials, CancellationToken token = default)
    {
        string query = @"INSERT INTO Credentials (Username, Password, LastLogIn, AccountStatus, EmployeeId)
                        VALUES (@Username, @Password, @LastLogIn, @AccountStatus, @EmployeeId);

                        UPDATE Employees 
                        SET IsCompleted = 1
                        WHERE EmployeeId = @EmployeeId";

        PasswordHasher<Credentials> hasher = new PasswordHasher<Credentials>();

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlTransaction transaction = connection.BeginTransaction())
            {
                await using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = credentials.Username;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hasher.HashPassword(credentials, credentials.Password);
                    command.Parameters.Add("@LastLogIn", SqlDbType.DateTime2).Value = DateTime.Now;
                    command.Parameters.Add("@AccountStatus", SqlDbType.NVarChar).Value = credentials.AccountStatus;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = credentials.EmployeeId;

                    try
                    {
                        int affectedRows = await command.ExecuteNonQueryAsync(token);
                        await transaction.CommitAsync(token);
                        return affectedRows > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync(token);
                        return false;
                    }
                }
            }
        }
    }

    public async Task UpdatePasswordHashAsync(int employeeId, string username, string password, CancellationToken token = default)
    {
        string query = @"UPDATE Credentials
                        SET Password = @Password
                        WHERE Username = @Username
                        AND EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                PasswordHasher<Credentials> hasher = new PasswordHasher<Credentials>();

                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hasher.HashPassword(null!, password);
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;

                await command.ExecuteNonQueryAsync(token);
            }
        }
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken token)
    {
        string query = @"SELECT COUNT(*)
                        FROM Credentials
                        WHERE Username = @Username";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

                return Convert.ToInt32(await command.ExecuteScalarAsync(token)) > 0;
            }
        }
    }
}
