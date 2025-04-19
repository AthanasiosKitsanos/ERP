using System;
using ErpProject.Helpers.Connection;
using ErpProject.Models.DTOModels;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmploymentDetailsServices;

public class EmploymentDetailsService
{
    public async Task<int> AddEmploymentDetailsAsync(int id, EmploymentDetailsDTO details, SqlConnection connection, SqlTransaction transaction)
    {
        if(details is null)
        {
            return 0;
        }

        string query = @"INSERT INTO EmploymentDetails (Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation, EmployeeId)
                        VALES (Position = @Position , Department = @Department, EmploymentStatus = @EmploymentStatus, HireDate = @HireDate, ContractType = @ContractType, WorkLocation = @WorkLocation, EmployeeId = @EmployeeId)";

        using(SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@Position", details.Position);
            command.Parameters.AddWithValue("@Department", details.Department);
            command.Parameters.AddWithValue("@EmploymentStatus", details.EmploymentStatus);
            command.Parameters.AddWithValue("@HireDate", details.HireDate);
            command.Parameters.AddWithValue("@ContractType", details.ContractType);
            command.Parameters.AddWithValue("@WorkLocation", details.WorkLocation);
            command.Parameters.AddWithValue("@EmployeeId", id);

            int affectedRows = await command.ExecuteNonQueryAsync();
            
            return affectedRows;
        }
    }
}