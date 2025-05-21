using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class IdentificationsServices
{
    private readonly Connection _connection;

    public IdentificationsServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<Identifications> GetIdentificationsAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        Identifications identifications = new Identifications();

        string query = @"SELECT TIN, WorkAuth, TaxInformation 
                        FROM Identifications
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        identifications.TIN = reader.GetString(reader.GetOrdinal("TIN"));
                        identifications.WorkAuth = reader.GetBoolean(reader.GetOrdinal("WorkAuth"));
                        identifications.TaxInformation = reader.GetString(reader.GetOrdinal("TaxInformation"));
                    }
                }
            }
        }

        return identifications;
    }
}
