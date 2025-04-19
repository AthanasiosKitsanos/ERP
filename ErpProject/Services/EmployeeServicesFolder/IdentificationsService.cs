using System;
using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models.DTOModels;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.IdentificationServices;

public class IdentificationsService
{
    private readonly Connection _connection;

    public IdentificationsService(Connection connection)
    {
        _connection = connection;
    }

    public async Task<int> AddIdentificationsAsync(int id, IdentificationsDTO identifications, SqlConnection connection, SqlTransaction transaction)
    {
        if(identifications is null)
        {
            return 0;
        }

        string query = @"INSERT INTO Identifications (TIN, WorkAuth, TaxInformation, EmployeeId)
                        VALUES (@TIN, @WorkAuth, @TaxInformation, @EmployeeId)";

        using(SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@TIN", identifications.TIN);
            command.Parameters.Add("@WorkAuth", SqlDbType.Bit).Value = identifications.WorkAuth;
            command.Parameters.AddWithValue("@TaxInformation", identifications.TaxInformation);
            command.Parameters.AddWithValue("@EmployeeId", id);

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows;
        }
    }
}
