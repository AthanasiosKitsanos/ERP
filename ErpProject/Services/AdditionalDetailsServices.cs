using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class AdditionalDetailsServices
{
    private readonly Connection _connection;

    public AdditionalDetailsServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<AdditionalDetails> GetAdditionalDetailsAsync(int id)
    {
        AdditionalDetails details = new AdditionalDetails();

        if(id == 0)
        {
            return null!;
        }

        string query = @"SELECT EmergencyNumbers, Education, Identifications, PersonalDocuments, MIME
                        FROM AdditionalDetails
                        WHERE EmployeeId = @EmployeeId";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        details.EmergencyNumbers = reader.GetString(reader.GetOrdinal("EmergencyNumbers"));
                        details.Education = reader.GetString(reader.GetOrdinal("Education"));
                    }
                }
            }
        }

        return details;
    }


}
