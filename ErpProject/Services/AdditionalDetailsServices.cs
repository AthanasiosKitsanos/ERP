using System.Data;
using ErpProject.Helpers;
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

        string query = @"SELECT EmergencyNumbers, Education, EmployeeId
                        FROM AdditionalDetails
                        WHERE EmployeeId = @EmployeeId";

        await using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        details.EmergencyNumbers = reader.GetString(reader.GetOrdinal("EmergencyNumbers"));
                        details.Education = reader.GetString(reader.GetOrdinal("Education"));
                        details.EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                    }
                }
            }
        }

        return details;
    }

    public async Task<bool> AddAdditionalDetailsAsync(int id, AdditionalDetails details)
    {
        if(id <= 0 || details is null || details.CertificationFile is null || details.PersonalDocumentsFile is null)
        {
            return false;
        }
        
        FileToByteArray arrayConverter = new FileToByteArray();

        string query = @"INSERT INTO AdditionalDetails (EmergencyNumbers, Education, EmployeeId)
                        VALUES (@EmergencyNumbers, @Education, @EmployeeId);

                        INSERT INTO Certifications (CertData, MIME, EmployeeId)
                        VALUES(@CertData, @CertMime, @EmployeeId);

                        INSERT INTO PersonalDocuments (DocData, MIME, EmployeeID)
                        VALUES (@DocData, @DocMime, @EmployeeId)";

        await using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlTransaction transaction = connection.BeginTransaction())
            {
                await using(SqlCommand command =  new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("@EmergencyNumbers", SqlDbType.NVarChar).Value = details!.EmergencyNumbers;
                    command.Parameters.Add("@Education", SqlDbType.NVarChar).Value = details.Education;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@CertData", SqlDbType.VarBinary).Value = await arrayConverter.AddToArray(details.CertificationFile);
                    command.Parameters.Add("@CertMime", SqlDbType.NVarChar).Value = details.CertificationFile.ContentType;
                    command.Parameters.Add("@DocData", SqlDbType.VarBinary).Value = await arrayConverter.AddToArray(details.PersonalDocumentsFile);
                    command.Parameters.Add("@DocMime", SqlDbType.NVarChar).Value = details.PersonalDocumentsFile.ContentType;
                
                    try
                    {
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        return affectedRows == 3;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }

    public async Task<bool> UpdateAdditionalDetailsAsync(int id, AdditionalDetails details)
    {
        FileToByteArray converter = new FileToByteArray();

        List<string> additions = new List<string>();

        List<SqlParameter> parameters = new List<SqlParameter>();



        if(!string.IsNullOrEmpty(details.EmergencyNumbers) && !string.IsNullOrWhiteSpace(details.EmergencyNumbers))
        {
            additions.Add("EmergencyNumbers = @EmergencyNumbers");
            parameters.Add(new SqlParameter("@EmergencyNumbers", SqlDbType.NVarChar){Value = details.EmergencyNumbers});
        }

        if(!string.IsNullOrEmpty(details.Education) && !string.IsNullOrWhiteSpace(details.Education))
        {
            additions.Add("Education = @Education");
            parameters.Add(new SqlParameter("@Education", SqlDbType.NVarChar){Value = details.Education});
        }

        string query = string.Empty;

        if(additions.Count > 0)
        {
            query = $@"UPDATE AdditionalDetails SET {string.Join(", ", additions)} WHERE EmployeeId = @EmployeeId;";
        }

        if(details.CertificationFile is not null)
        {
            query += @"INSERT INTO Certifications (CertData, MIME, EmployeeId)
                            VALUES(@CertData, @CertMime, @EmployeeId);";

            parameters.Add(new SqlParameter("@CertData", SqlDbType.VarBinary){Value = await converter.AddToArray(details.CertificationFile)});
            parameters.Add(new SqlParameter("@CertMime", SqlDbType.NVarChar){Value = details.CertificationFile.ContentType});
        }

        if(details.PersonalDocumentsFile is not null)
        {
            query += @"INSERT INTO PersonalDocuments (DocData, MIME, EmployeeID)
                            VALUES (@DocData, @DocMime, @EmployeeId)";
            parameters.Add(new SqlParameter("@DocData", SqlDbType.VarBinary){Value = await converter.AddToArray(details.PersonalDocumentsFile)});
            parameters.Add(new SqlParameter("@DocMime", SqlDbType.NVarChar){Value = details.PersonalDocumentsFile.ContentType});
        }

        if(additions.Count == 0 && parameters.Count == 0)
        {
            return false;
        }

        await using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using(SqlTransaction transaction = connection.BeginTransaction())
            {
                await using(SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("EmployeeId", SqlDbType.Int).Value = id;
                    command.Parameters.AddRange(parameters.ToArray());

                    try
                    {
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        return affectedRows > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }
}