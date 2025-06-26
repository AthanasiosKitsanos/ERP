using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class AdditionalDetailsRepository : IAdditionalDetailsRepository
{
    private readonly Connection _connection;

    public AdditionalDetailsRepository(Connection connection)
    {
        _connection = connection;
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

        return null!;
    }

    public Task<bool> UpdateAsync(AdditionalDetails details, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
