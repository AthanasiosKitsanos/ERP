using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class EmploymentDetailsServices
{
    private readonly Connection _connection;

    public EmploymentDetailsServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<EmploymentDetails> GetEmploymentDetailsAsync(int id)
    {
        if(id <= 0)
        {
            return null!;
        }

        EmploymentDetails details = new EmploymentDetails();

        string query = @"SELECT Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation, EmployeeId
                        FROM EmploymentDetails 
                        WHERE EmployeeId = @EmployeeId";

        await using(SqlConnection connection =  new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                
                await using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        details.Position = reader.GetString(reader.GetOrdinal("Position"));
                        details.Department = reader.GetString(reader.GetOrdinal("Department"));
                        details.EmploymentStatus = reader.GetString(reader.GetOrdinal("EmploymentStatus"));
                        details.HireDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("HireDate")));
                        details.ContractType = reader.GetString(reader.GetOrdinal("ContractType"));
                        details.WorkLocation = reader.GetString(reader.GetOrdinal("WorkLocation"));
                        details.EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                    }
                }
            }
        }

        return details;
    }

    public async Task<bool> AddEmploymentDetailsAsync(int id, EmploymentDetails details)
    {
        if(id <= 0)
        {
            return false;
        }

        string query = @"INSERT INTO EmploymentDetails (Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation, EmployeeId)
                        VALUES (@Position, @Department, @EmploymentStatus, @HireDate, @ContractType, @WorkLocation, @EmployeeId)";

        await using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = details.Position;
                command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = details.Department;
                command.Parameters.Add("@EmploymentStatus", SqlDbType.NVarChar).Value = details.EmploymentStatus;
                command.Parameters.Add("@HireDate", SqlDbType.Date).Value = details.HireDate;
                command.Parameters.Add("@ContractType", SqlDbType.NVarChar).Value = details.ContractType;
                command.Parameters.Add("@WorkLocation", SqlDbType.NVarChar).Value = details.WorkLocation;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();
                
                return affectedRows > 0;
            }
        }
    }

    public async Task<bool> UpdateEmploymentDetailsAsync(int id, EmploymentDetails details)
    {
        if(id <= 0)
        {
            return false;
        }

        List<string> additions = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if(!string.IsNullOrEmpty(details.Position) && !string.IsNullOrWhiteSpace(details.Position))
        {
            additions.Add("Position = @Position");
            parameters.Add(new SqlParameter("@Position", SqlDbType.NVarChar){Value = details.Position});
        }

        if(!string.IsNullOrEmpty(details.Department) && !string.IsNullOrWhiteSpace(details.Department))
        {
            additions.Add("Department = @Department");
            parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar){Value = details.Department});
        }

        if(!string.IsNullOrEmpty(details.EmploymentStatus) && !string.IsNullOrWhiteSpace(details.EmploymentStatus))
        {
            additions.Add("EmploymentStatus = @EmploymentStatus");
            parameters.Add(new SqlParameter("@EmploymentStatus", SqlDbType.NVarChar){Value = details.EmploymentStatus});
        }

        additions.Add("HireDate = @HireDate");
        parameters.Add(new SqlParameter("@HireDate", SqlDbType.Date){Value = details.HireDate});

        if(!string.IsNullOrEmpty(details.ContractType) && !string.IsNullOrWhiteSpace(details.ContractType))
        {
            additions.Add("ContractType = @ContractType");
            parameters.Add(new SqlParameter("@ContractType", SqlDbType.NVarChar){Value = details.ContractType});
        }

        if(!string.IsNullOrEmpty(details.WorkLocation) && !string.IsNullOrWhiteSpace(details.WorkLocation))
        {
            additions.Add("WorkLocation = @WorkLocation");
            parameters.Add(new SqlParameter("@WorkLocation", SqlDbType.NVarChar){Value = details.WorkLocation});
        }

        string query = @$"UPDATE EmploymentDetails 
                        SET {string.Join(", ", additions)}
                        WHERE EmployeeId = @EmployeeId";

        if(additions.Count <= 1 || parameters.Count <= 1)
        {
            return false;
        }

        await using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.AddRange(parameters.ToArray());

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }
}
