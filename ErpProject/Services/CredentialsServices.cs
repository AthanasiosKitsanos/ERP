using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class CredentialsServices
{
    private readonly Connection _connection;

    public CredentialsServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> CreateCredentialsAsync(int id, Credentials credentials)
    {
        if (id <= 0)
        {
            return false;
        }

        string query = @"INSERT INTO Credentials (Username, Password, LastLogIn, AccountStatus, EmployeeId)
                        VALUES (@Username, @Password, @LastLogIn, @AccountStatus, @EmployeeId);

                        UPDATE Employees SET IsCompleted = 1 WHERE EmployeeId = @EmployeeId";

        PasswordHasher<Credentials> hasher = new PasswordHasher<Credentials>();

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlTransaction transaction = connection.BeginTransaction())
            {
                await using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = credentials.Username;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hasher.HashPassword(credentials, credentials.Password);
                    command.Parameters.Add("@LastLogIn", SqlDbType.DateTime2).Value = DateTime.Now;
                    command.Parameters.Add("@AccountStatus", SqlDbType.NVarChar).Value = credentials.AccountStatus;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    try
                    {
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        return affectedRows > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        string query = @"SELECT COUNT(*)
                        FROM Credentials
                        WHERE Username = @Username";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
        }
    }


    public async Task UpdatePasswordHashAsync(string username, string password)
    {
        string query = @"UPDATE Credentials
                        SET Password = @Password
                        WHERE Username = @Username";

        PasswordHasher<Credentials> hasher = new PasswordHasher<Credentials>();

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hasher.HashPassword(null!, password);
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}