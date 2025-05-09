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

        string query = @"SELECT Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation
                        FROM EmploymentDetails WHERE EmployeeId = @EmployeeId";

        using(SqlConnection connection =  new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        details.Position = reader.GetString(reader.GetOrdinal("Position"));
                        details.Department = reader.GetString(reader.GetOrdinal("Department"));
                        details.EmploymentStatus = reader.GetString(reader.GetOrdinal("EmploymentStatus"));
                        details.HireDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("HireDate")));
                        details.ContractType = reader.GetString(reader.GetOrdinal("ContractType"));
                        details.WorkLocation = reader.GetString(reader.GetOrdinal("WorkLocation"));
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

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = details.Position;
                command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = details.Department;
                command.Parameters.Add("@EmployeeStatus", SqlDbType.NVarChar).Value = details.EmploymentStatus;
                command.Parameters.Add("@HireDate", SqlDbType.Date).Value = details.HireDate;
                command.Parameters.Add("@ContractType", SqlDbType.NVarChar).Value = details.ContractType;
                command.Parameters.Add("@WorkLocation", SqlDbType.NVarChar).Value = details.WorkLocation;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();
                
                return affectedRows > 0;
            }
        }
    }
}
