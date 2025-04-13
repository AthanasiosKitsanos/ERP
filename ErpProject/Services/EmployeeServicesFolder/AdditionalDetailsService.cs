using ErpProject.Models.DTOModels;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmployeeServicesFolder;

public class AdditionalDetailsService
{
    public async Task<bool> AddAdditionalDetailsAsync(int id, AdditionalDetailsDTO details, SqlConnection connection, SqlTransaction transaction)
    {
        if (details is null)
        {
            return false;
        }

        string query = @"ISNERT INTO AdditionalDetails (EmergencyNumbers, Education, Certifications, EmployeeId)
                        VALUES (@EmergencyNumbers, @Education, @Certifications, @EmployeeId)";

        int affectedRows = 0;

        using (SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@EmergencyNumbers", details.EmergencyNumbers);
            command.Parameters.AddWithValue("@Education", details.Education);
            command.Parameters.AddWithValue("@EmployeeId", id);

            affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }
    }

    public async Task<bool> AddCertificationsAsync(int id, CertificationDTO certificationList, SqlConnection connection, SqlTransaction transaction)
    {
        if (certificationList is null || certificationList.CertificationPaths.Count == 0)
        {
            return false;
        }
        
        int affectedRows = 0;

        string query = @"INSERT INTO Certifications (CertificationPath, EmployeeId)
                        VALUES (@CertificationPath, @EmployeeId)";

        using (SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            foreach (string url in certificationList.CertificationPaths)
            {
                command.Parameters.Clear();

                command.Parameters.AddWithValue("@CertificationPath", url);
                command.Parameters.AddWithValue("@EmployeeId", id);
            
                affectedRows += await command.ExecuteNonQueryAsync();
            }
        }
        
        return affectedRows > 0;
    }

    public async Task<bool> AddPersonalDocumentsAsync(int id, PersonalDocumentsDTO personalDocumentsList, SqlConnection connection, SqlTransaction transaction)
    {
        if (personalDocumentsList is null || personalDocumentsList.DocumentsPaths.Count == 0)
        {
            return false;
        }
        
        int affectedRows = 0;

        string query = @"INSERT INTO PersonalDocuments (DocumentsPaths, EmployeeId)
                        VALUES (@DocumentsPaths, @EmployeeId)";

        using (SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            foreach (string url in personalDocumentsList.DocumentsPaths)
            {
                command.Parameters.Clear();

                command.Parameters.AddWithValue("@DocumentsPaths", url);
                command.Parameters.AddWithValue("@EmployeeId", id);
            
                affectedRows += await command.ExecuteNonQueryAsync();
            }
        }
        
        return affectedRows > 0;
    }
}