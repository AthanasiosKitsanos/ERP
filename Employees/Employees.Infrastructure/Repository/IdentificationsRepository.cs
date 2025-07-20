using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class IdentificationsRepository : IIdentificationRepository
{
    private readonly Connection _connection;

    public IdentificationsRepository(Connection connection)
    {
        _connection = connection;
    }
    public async Task<bool> CreateAsync(int id, Identifications details, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        string query = @"INSERT INTO dbo.Identifications (TIN, WorkAuth, TaxInformation, EmployeeId)
                        VALUES (@TIN, @WorkAuth, @TaxInformation, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TIN", SqlDbType.NVarChar).Value = details.TIN;
                command.Parameters.Add("@WorkAuth", SqlDbType.NVarChar).Value = details.WorkAuth;
                command.Parameters.Add("@TaxInformation", SqlDbType.NVarChar).Value = details.TaxInformation;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }

    public async Task<Identifications> GetByIdAsync(int id, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        string query = @"SELECT TIN, WorkAuth, TaxInformation
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (await reader.ReadAsync(token))
                    {
                        return new Identifications
                        {
                            TIN = reader.GetString(0),
                            WorkAuth = reader.GetString(1),
                            TaxInformation = reader.GetString(2)
                        };
                    }
                }
            }
        }

        return new Identifications();
    }

    public async Task<bool> UpdateAsync(int id, Identifications details, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        
        List<string> variables = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(details.TIN))
        {
            variables.Add("TIN = @TIN");
            parameters.Add(new SqlParameter("@TIN", SqlDbType.NVarChar) { Value = details.TIN });
        }

        if (!string.IsNullOrEmpty(details.WorkAuth))
        {
            variables.Add("WorkAuth = @WorkAuth");
            parameters.Add(new SqlParameter("@WorkAuth", SqlDbType.NVarChar) { Value = details.WorkAuth });
        }

        if (!string.IsNullOrEmpty(details.TaxInformation))
        {
            variables.Add("TaxInfomration = @TaxInfomration");
            parameters.Add(new SqlParameter("@TaxInformation", SqlDbType.NVarChar) { Value = details.TaxInformation });
        }

        string query = $@"UPDATE dbo.Identifications
                        SET {string.Join(", ", variables)}
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.AddRange(parameters.ToArray());

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }
}
