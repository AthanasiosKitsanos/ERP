using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class PhotoServices
{
    private readonly Connection _connection;

    public PhotoServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<Photo> GetEmployeePhotoAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        Photo photo = new Photo();

        string query = @"SELECT Photograph, MIME
                        FROM Employees
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
                        photo.Photograph = await reader.GetFieldValueAsync<byte[]>(reader.GetOrdinal("Photograph"));
                        photo.MIME = reader.GetString(reader.GetOrdinal("MIME"));
                    }
                }
            }
        }

        return photo;
    }
}
