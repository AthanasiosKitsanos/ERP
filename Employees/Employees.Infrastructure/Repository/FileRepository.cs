using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class FileRepository : IFileRepository
{
    private readonly Connection _connection;

    public FileRepository(Connection connection)
    {
        _connection = connection;
    }

    public async Task<Photo> GetPhotoAsync(int id, CancellationToken token = default)
    {
        string query = @"SELECT Photograph, Mime
                        FROM dbo.Employees
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

                        return new Photo
                        {
                            Photograph = await reader.GetFieldValueAsync<byte[]>(0),
                            MIME = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return null!;
    }
}
