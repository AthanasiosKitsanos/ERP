using Microsoft.Data.SqlClient;

namespace ErpProject.BackgroundServices;

public class RefreshTokenCleanUpService : BackgroundService
{
    private readonly string _connectionString;

    public RefreshTokenCleanUpService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string query = @"DELETE TOP(100)
                        FROM RefreshToken
                        WHERE ExpiresAt < GETUTCDATE()
                        OR RevokedAt IS NOT NULL";

        while (!stoppingToken.IsCancellationRequested)
        {
            await using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                await using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
