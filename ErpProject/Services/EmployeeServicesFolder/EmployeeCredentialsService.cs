using ErpProject.Helpers.Connection;
using ErpProject.Models.AccountStatusModel;
using ErpProject.Models.DTOModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmployeeCredentialsServices;

public class EmployeeCredentialsService
{
    private readonly Connection _connection;

    public EmployeeCredentialsService(Connection connection)
    {
        _connection = connection;
    }

    public async Task<List<AccountStatus>> GetAccountStatusAsync()
    {
        List<AccountStatus> statusList = new List<AccountStatus>();

        string query = @"SELECT * FROM AccountStatus";

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

                        statusList.Add(status);
                    }
                }
            }
        }

        return statusList;
    }

    public async Task<int> AddCredentialsToEmployeeAsync(int id, EmployeeCredentialsDTO credentials, AccountStatusDTO status, SqlConnection connection, SqlTransaction transaction)
    {
        if(credentials is null || status is null)
        {
            return 0;
        }

        PasswordHasher<object> hasher = new PasswordHasher<object>();

        string query = @"INSERT INTO EmployeeCredentials (Username, Password, AccountStatusId, EmployeeId)
                        VALUES (@Id, @Username, @Password, @AccountStatusId, @EmployeeId)";

        using(SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@Username", credentials.Username);
            command.Parameters.AddWithValue("@Password", hasher.HashPassword(null!, credentials.Password));
            command.Parameters.AddWithValue("@AccountStatusId", status.SelectedStatus);
            command.Parameters.AddWithValue("@EmployeeId", id);

            await command.ExecuteNonQueryAsync();
        }

        return 1;
    }
}