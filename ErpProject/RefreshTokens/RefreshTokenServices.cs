using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.RefreshTokens.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.RefreshTokens;

public class RefreshTokenServices
{
    private readonly Connection _connection;

    public RefreshTokenServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> GenerateRefreshTokenAsync(int id, string ipAddress, bool rememberMe)
    {
        if (id <= 0 || string.IsNullOrEmpty(ipAddress))
        {
            return false;
        }

        string query = @"INSERT INTO RefreshToken (Id, Token, CreatedAt, ExpiresAt, CreatedByIp, EmployeeId)
                        VALUES (@Id, @Token, @CreatedAt, @ExpiresAt, @CreatedByIp, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString("N");
                command.Parameters.Add(@"CreatedAt", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                command.Parameters.Add(@"ExpiresAt", SqlDbType.DateTime2).Value = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(15);
                command.Parameters.Add("@CreatedByIp", SqlDbType.NVarChar).Value = ipAddress;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<string> GetRefreshTokenAsync(int id, string ipAddress)
    {
        if (id <= 0)
        {
            return string.Empty;
        }

        string token = string.Empty;

        string query = @"SELECT TOP 1 Token
                        FROM RefreshToken
                        WHERE EmployeeId = @EmployeeId
                        AND CreatedByIp = @CreatedByIp
                        AND RevokedAt IS NULL
                        AND ExpiresAt > GETUTCDATE()
                        ORDER BY CreatedAt DESC";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.Add("@CreatedByIp", SqlDbType.NVarChar).Value = ipAddress;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        token = reader.GetString(reader.GetOrdinal("Token"));
                    }
                }
            }
        }

        return token;
    }

    public async Task<int> ValidateRefreshTokenAsync(string token, string currentIpAddress)
    {
        if (string.IsNullOrEmpty(token))
        {
            return 0;
        }

        int id = 0;

        string query = @"SELECT EmployeeId
                        FROM RefreshToken
                        WHERE Token = @Token
                        AND RevokedAt IS NULL
                        AND ExpiresAt > GETUTCDATE()
                        AND CreatedByIp = @CreatedByIp";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;
                command.Parameters.Add("@CreatedByIp", SqlDbType.NVarChar).Value = currentIpAddress;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                    }
                }
            }
        }

        return id;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, string ipAddress)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ipAddress))
        {
            return false;
        }

        string query = @"UPDATE RefreshToken 
                        SET RevokedAt = @RevokedAt,
                        RevokedByIp = @RevokedByIp
                        WHERE Token = @Token";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@RevokedAt", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;
                command.Parameters.Add("@RevokedByIp", SqlDbType.NVarChar).Value = ipAddress;

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}