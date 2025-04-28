using System.Data;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class CredentialsServices
{
    private readonly Connection _connection;

    public CredentialsServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> CreateCredentialsAsync(int id, Credentials credentials)
    {
        if(id <= 0)
        {
            return false;
        }

        string query = @"INSERT INTO Credentials (Username, Password, LastLogIn, AccountStatusId, EmployeeId)
                        VALUES (@Username, @Password, @LastLogIn, @AccountStatusId, @EmployeeId);
                        UPDATE Employees SET IsCompleted = 1 WHERE Id = @Id";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = credentials.Username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = credentials.Password;
                command.Parameters.Add("@LastLogIn", SqlDbType.DateTime2).Value = DateTime.Now;
                command.Parameters.Add("@AccountStatusId", SqlDbType.Int).Value = credentials.AccountStatusId;
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    public async Task<List<AccountStatus>> GetStatusNamesAsync()
    {
        List<AccountStatus> statusNamesList = new List<AccountStatus>();

        string query = @"SELECT *
                        FROM AccountStatus";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> param = new Dictionary<string, int>
                    {
                        {"Id", reader.GetOrdinal("Id")},
                        {"StatusName", reader.GetOrdinal("StatusName")}
                    };

                    while(await reader.ReadAsync())
                    {
                        AccountStatus status = new AccountStatus
                        {
                            Id = reader.GetInt32(param["Id"]),
                            StatusName = reader.GetString(param["StatusName"])
                        };

                        statusNamesList.Add(status);
                    }
                }
            }
        }

        return statusNamesList;
    }
}