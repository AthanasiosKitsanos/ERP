using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class EmploymentDetailsRepository : IEmploymentDetailsRepository
{
    private readonly Connection _connection;

    public EmploymentDetailsRepository(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> CreateAsync(EmploymentDetails details, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        string query = @"INSERT INTO dbo.EmploymentDetails (Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation, EmployeeId)
                        VALUES (@Position, @Department, @EmploymentStatus, @HireDate, @ContractType, @WorkLocation, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = details.Position;
                command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = details.Department;
                command.Parameters.Add("@EmploymentStatus", SqlDbType.NVarChar).Value = details.EmploymentStatus;
                command.Parameters.Add("@HireDate", SqlDbType.Date).Value = details.HireDate;
                command.Parameters.Add("@ContractType", SqlDbType.NVarChar).Value = details.ContractType;
                command.Parameters.Add("@WorkLocation", SqlDbType.NVarChar).Value = details.WorkLocation;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = details.EmployeeId;

                token.ThrowIfCancellationRequested();

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }

    public async Task<EmploymentDetails> GetByIdAsync(int id, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        string query = @"SELECT Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation
                        FROM dbo.EmploymentDetails
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                token.ThrowIfCancellationRequested();

                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (await reader.ReadAsync())
                    {
                        return new EmploymentDetails
                        {
                            Position = reader.GetString(0),
                            Department = reader.GetString(1),
                            EmploymentStatus = reader.GetString(2),
                            HireDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                            ContractType = reader.GetString(4),
                            WorkLocation = reader.GetString(5)
                        };
                    }
                }
            }
        }

        return new EmploymentDetails();
    }

    public async Task<bool> UpdateAsync(EmploymentDetails details, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        List<string> variables = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(details.Position))
        {
            variables.Add("Position = @Position");
            parameters.Add(new SqlParameter("@Position", SqlDbType.NVarChar) { Value = details.Position });
        }

        if (!string.IsNullOrEmpty(details.Department))
        {
            variables.Add("Department = @Department");
            parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar) { Value = details.Department });
        }

        if (!string.IsNullOrEmpty(details.EmploymentStatus))
        {
            variables.Add("EmploymentStatus = @EmploymentStatus");
            parameters.Add(new SqlParameter("@EmploymentStatus", SqlDbType.NVarChar) { Value = details.EmploymentStatus });
        }

        variables.Add("HireDate = @HireDate");
        parameters.Add(new SqlParameter("@HireDate", SqlDbType.Date) { Value = details.HireDate });

        if (!string.IsNullOrEmpty(details.ContractType))
        {
            variables.Add("ContractType = @ContractType");
            parameters.Add(new SqlParameter("@ContractType", SqlDbType.NVarChar) { Value = details.ContractType });
        }

        if (!string.IsNullOrEmpty(details.WorkLocation))
        {
            variables.Add("WorkLocation = @WorkLocation");
            parameters.Add(new SqlParameter("@WorkLocation", SqlDbType.NVarChar) { Value = details.WorkLocation });
        }

        if (variables.Count < 1)
        {
            return false;
        }

        string query = $@"UPDATE dbo.EmploymentDetails
                        SET {string.Join(", ", variables)}
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                token.ThrowIfCancellationRequested();

                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = details.EmployeeId;
                command.Parameters.AddRange(parameters.ToArray());

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }
}
