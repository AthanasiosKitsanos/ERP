using System;
using ErpProject.Helpers.Connection;
using ErpProject.Models.AdditionalDetailsModel;
using ErpProject.Models.DTOModels;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmployeeServicesFolder;

public class AdditionalDetailsService
{
    private readonly Connection _connection;

    public AdditionalDetailsService(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> AddAdditionalDetailsAsync(AdditionalDetailsDTO details, int id, SqlConnection connection, SqlTransaction transaction)
    {
        if(details is null)
        {
            return false;
        }

        string query = @"ISNERT INTO AdditionalDetails (EmergencyNumbers, Education, Certifications, PersonalDocuments, EmployeeId)
        VALUES (@EmergencyNumbers, @Education, @Certifications, @PersonalDocuments, @EmployeeId)";

        using(SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@EmergencyNumbers", details.EmergencyNumbers);
            command.Parameters.AddWithValue("@Education", details.Education);
            command.Parameters.AddWithValue("@Certifications", details.Certifications);
            command.Parameters.AddWithValue("@PersonalDocuments", details.PersonalDocuments);
            command.Parameters.AddWithValue("@EmployeeId", id);

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }
    }
}
