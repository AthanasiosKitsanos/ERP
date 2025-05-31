using System.Data;
using System.Security.Cryptography;
using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;

namespace ErpProject.RefreshTokens;

public class RefreshTokenServices
{
    private readonly Connection _connection;

    public RefreshTokenServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task CreateRefreshToken(int employeeId, string ipAddress)
    {

        string query = @"INSERT INTO RefreshToken (Id, Token, ExpiresAt, CreatedAt, CreatedByIp, EmployeeId)
                        VALUES (@Id, @Token, @ExpiresAt, @CreatedAt, @CreatedByIp, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = GenerateSecureToken();
                command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2).Value = DateTime.UtcNow.AddDays(7);
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                command.Parameters.Add("@CreatedByIp", SqlDbType.NVarChar).Value = ipAddress;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    private static string GenerateSecureToken()
    {
        byte[] randomBytes = new byte[64];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }

    public async Task<RefreshToken> GetValidatedToekAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null!;
        }

        string query = @"SELECT Id, Token, ExpiresAt, RevokedAt, EmployeeId
                        FROM RefreshToken
                        WHERE Token = @Token";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = token;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {

                        DateTime expiresAt = reader.GetDateTime(reader.GetOrdinal("ExpiresAt"));
                        int revokedAt = reader.GetOrdinal("RevokedAt");

                        if (expiresAt < DateTime.UtcNow || !reader.IsDBNull(revokedAt))
                        {
                            return null!;
                        }

                        return new RefreshToken
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Token = reader.GetString(reader.GetOrdinal("Token")),
                            ExpiresAt = expiresAt,
                            RevokedAt = reader.IsDBNull(revokedAt) ? null : reader.GetDateTime(revokedAt)
                        };
                    }
                }
            }

            return null!;
        }
    }
}