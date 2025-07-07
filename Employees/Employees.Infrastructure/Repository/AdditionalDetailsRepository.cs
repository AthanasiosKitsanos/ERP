using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Employees.Infrastructure.Repository;

public class AdditionalDetailsRepository : IAdditionalDetailsRepository
{
    private readonly Connection _connection;
    private readonly ILogger<AdditionalDetailsRepository> _logger;

    public AdditionalDetailsRepository(Connection connection, ILogger<AdditionalDetailsRepository> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        string query = @"INSERT INTO dbo.AdditionalDetails (EmergencyNumbers, Education, EmployeeId)
                        VALUES (@EmergencyNumbers, @Education, @EmployeeId);
                        
                        INSERT INTO dbo.Certifications (CertData, EmployeeId, MIME)
                        VALUES (@CertData, @EmployeeId, @CertMIME);
                        
                        INSERT INTO dbo.PersonalDocuments (DocData, EmployeeId, MIME)
                        VALUES (@DocData, @EmployeeId, @DocMIME)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlTransaction transaction = connection.BeginTransaction())
            {
                await using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = details.EmployeeId;
                    command.Parameters.Add("@EmergencyNumbers", SqlDbType.NVarChar).Value = details.EmergencyNumbers;
                    command.Parameters.Add("@Education", SqlDbType.NVarChar).Value = details.Education;
                    command.Parameters.Add("@CertData", SqlDbType.VarBinary).Value = details.Certifications;
                    command.Parameters.Add("@CertMIME", SqlDbType.NVarChar).Value = details.CertMime;
                    command.Parameters.Add("@DocData", SqlDbType.VarBinary).Value = details.PersonalDocuments;
                    command.Parameters.Add("@DocMIME", SqlDbType.NVarChar).Value = details.DocMime;

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        _logger.LogWarning("There was a problem while adding the additional");
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }

    public async Task<AdditionalDetails> GetAsync(int id, CancellationToken token = default)
    {
        string query = @"SELECT EmergencyNumbers, Education
                        FROM dbo.AdditionalDetails
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (await reader.ReadAsync(token))
                    {
                        token.ThrowIfCancellationRequested();

                        return new AdditionalDetails
                        {
                            EmergencyNumbers = reader.GetString(0),
                            Education = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return new AdditionalDetails();
    }

    public async Task<bool> UpdateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        List<string> variables = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(details.EmergencyNumbers))
        {
            variables.Add("EmergencyNumbers = @EmergencyNumbers");
            parameters.Add(new SqlParameter("@EmergencyNumbers", SqlDbType.NVarChar) { Value = details.EmergencyNumbers });
        }

        if (!string.IsNullOrEmpty(details.Education))
        {
            variables.Add("Education = @Education");
            parameters.Add(new SqlParameter("@Education", SqlDbType.NVarChar) { Value = details.Education });
        }

        if (variables.Count == 0)
        {
            return false;
        }

        string query = $@"UPDATE dbo.AdditionalDetails
                        SET {string.Join(", ", variables)}
                        WHERE EmployeeId = @EmployeeId";


        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = details.EmployeeId;
                command.Parameters.AddRange(parameters.ToArray());

                return await command.ExecuteNonQueryAsync(token) > 0;
            }
        }
    }
}
