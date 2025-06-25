using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class AdditionalDetailsRepository : IAdditionalDetailsRepository
{
    private readonly Connection _connection;

    public AdditionalDetailsRepository(Connection connection)
    {
        _connection = connection;
    }

    public Task<bool> CreateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<AdditionalDetails> GetAsync(int id, CancellationToken token = default)
    {
        string query = @"SELECT EmergencyNumbers, Education
                        FROM dbo.AdditionalDetails
                        WHERE EmployeeId = @EmployeeId";

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
                        token.ThrowIfCancellationRequested();

                        return new AdditionalDetails
                        {
                            EmergencyNumbers = reader.GetString(0),
                            Education = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null!;
    }

    public Task<bool> UpdateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
