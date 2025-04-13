using ErpProject.Models.DTOModels;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmployeeServicesFolder;

public class AdditionalDetailsService
{
    public async Task<bool> AddAdditionalDetailsAsync(int id, AdditionalDetailsDTO details, SqlConnection connection, SqlTransaction transaction)
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