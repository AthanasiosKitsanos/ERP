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
                        identifications.WorkAuth = reader.GetString(reader.GetOrdinal("WorkAuth"));
                        identifications.TaxInformation = reader.GetString(reader.GetOrdinal("TaxInformation"));
                    }
                }
            }
        }

        return identifications;
    }

    public async Task<bool> AddIdentificationsAsync(int id, Identifications identifications)
    {
        if (id <= 0)
        {
            return false;
        }

        string query = @"INSERT INTO Identifications (TIN, WorkAuth, TaxInformation, EmployeeId)
                        VALUES (@TIN, @WorkAuth, @TaxInformation, @EmployeeId)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TIN", SqlDbType.NVarChar).Value = identifications.TIN;
                command.Parameters.Add("@WorkAuth", SqlDbType.NVarChar).Value = identifications.WorkAuth;
                command.Parameters.Add("@TaxInformation", SqlDbType.NVarChar).Value = identifications.TaxInformation;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    public async Task<bool> EditIdentificationsAsync(int id, Identifications identifications)
    {
        if (id <= 0 || identifications is null)
        {
            return false;
        }

        List<string> additions = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(identifications.TIN) || !string.IsNullOrWhiteSpace(identifications.TIN))
        {
            additions.Add("TIN = @TIN");
            parameters.Add(new SqlParameter("@TIN", SqlDbType.NVarChar) { Value = identifications.TIN });
        }

        if (!string.IsNullOrEmpty(identifications.WorkAuth) || !string.IsNullOrWhiteSpace(identifications.WorkAuth))
        {
            additions.Add("WorkAuth = @WorkAuth");
            parameters.Add(new SqlParameter("@WorkAuth", SqlDbType.NVarChar) { Value = identifications.WorkAuth });
        }

        if (!string.IsNullOrEmpty(identifications.TaxInformation) || !string.IsNullOrWhiteSpace(identifications.TaxInformation))
        {
            additions.Add("TaxInformation = @TaxInformation");
            parameters.Add(new SqlParameter("@TaxInformation", SqlDbType.NVarChar) { Value = identifications.TaxInformation });
        }

        string query = $@"UPDATE Identifications
                        SET {string.Join(", ", additions)}
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.AddRange(parameters.ToArray());

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    public async Task<bool> TinExistsAsync(string tin)
    {
        if (string.IsNullOrEmpty(tin))
        {
            return false;
        }

        string query = @"SELECT COUNT(*)
                        FROM Identifications
                        WHERE TIN = @TIN";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TIN", SqlDbType.NVarChar).Value = tin;

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
        }
    }
}
