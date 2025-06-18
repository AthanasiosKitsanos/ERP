using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ErpProject.BackgroundServices;

public class RegistrationCleanUpService: BackgroundService
{
    private readonly string _connectionString;

    public RegistrationCleanUpService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string query = @"DELETE TOP(100) FROM Employees 
                        WHERE IsCompleted = 0 AND CreatedAt < DATEADD(MINUTE, -10, GETDATE())";

        while(!stoppingToken.IsCancellationRequested)
        {
            await using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(stoppingToken);

                await using(SqlCommand command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);   
        }
    }
}