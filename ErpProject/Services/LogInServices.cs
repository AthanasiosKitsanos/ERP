using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class LogInServices
{
    private readonly Connection _connection;
    private readonly CredentialsServices _credentials;

    public LogInServices(Connection connection, CredentialsServices credentials)
    {
        _connection = connection;
        _credentials = credentials;
    }

    public async Task<bool> LogInAsync(LogIn login)
    {
        if (login is null)
        {
            return false;
        }

        string dbPassword = string.Empty;

        string query = @"SELECT Password
                        FROM Credentials
                        WHERE Username = @Username";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = login.Username;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dbPassword = reader.GetString(reader.GetOrdinal("Password"));
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(dbPassword))
        {
            return false;
        }

        PasswordHasher<Credentials> hasher = new PasswordHasher<Credentials>();

        PasswordVerificationResult result = hasher.VerifyHashedPassword(null!, dbPassword, login.Password);

        bool isValid = result != PasswordVerificationResult.Failed;

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            await _credentials.UpdatePasswordHashAsync(login.Username, login.Password);
        }

        return isValid;
    }

    public async Task<LoggedInData> GetLoggedInDataByUsernameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return null!;
        }

        string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, r.RoleName
                        FROM Employees e
                        JOIN Credentials c ON c.EmployeeId = e.EmployeeId
                        JOIN RoleEmployee re ON re.EmployeeId = e.EmployeeId
                        JOIN Roles r ON r.Id = re.RoleId
                        WHERE Username = @Username";

        LoggedInData data = new LoggedInData();

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    await using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.Add(@"Username", SqlDbType.NVarChar).Value = username;

                        await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                data.Id = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                                data.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                data.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                data.RoleName = reader.GetString(reader.GetOrdinal("RoleName"));
                            }
                        }
                    }

                    if (data.Id == 0)
                    {
                        await transaction.RollbackAsync();
                        return null!;
                    }

                    query = @"UPDATE Credentials
                            SET LastLogin = @LastLogin
                            WHERE Username = @Username";

                    await using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.Add("@LastLogin", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;

                        await command.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        return data;
    }

    public async Task<LoggedInData> GetLoggedInDataByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, r.RoleName
                        FROM Employees e
                        JOIN RoleEmployee re ON re.EmployeeId = e.EmployeeId
                        JOIN Roles r ON r.Id = re.RoleId
                        WHERE e.EmployeeId = @EmployeeId";

        LoggedInData data = new LoggedInData();

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
                        data.Id = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                        data.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        data.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        data.RoleName = reader.GetString(reader.GetOrdinal("RoleName"));
                    }
                }
            }
        }

        return data;
    }
}