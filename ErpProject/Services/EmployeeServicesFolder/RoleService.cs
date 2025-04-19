using ErpProject.Models.RolesModel;
using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;
using ErpProject.Models.DTOModels;

namespace ErpProject.Services.RoleServices;

public class RoleService
{
    private readonly Connection _connection;

    public RoleService(Connection connection)
    {
        _connection = connection;
    }

    public async Task<List<Roles>> GetAllRolesAsync()
    {
        List<Roles> rolesList = new List<Roles>();

        string query = @"SELECT *
                        FROM Roles";


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
                        {"RoleName", reader.GetOrdinal("RoleName")},
                    };

                    while(await reader.ReadAsync())
                    {
                        Roles role = new Roles
                        {
                            Id = reader.GetInt32(param["Id"]),
                            RoleName = reader.GetString(param["RoleName"])
                        };  

                        rolesList.Add(role);
                    }
                }
            }
        }

        return rolesList;
    }

    public async Task<int> AddRoleToEmployeeAsync(int id, RolesDTO roles, SqlConnection connection, SqlTransaction transaction)
    {
        if(roles.SelectedRole == 0)
        {
            return 0;
        }

        string query = @"INSERT INTO RoleEmployee (RoleId, EmployeeId)
                        VALUES (@RoleId, @EmployeeId)";

        using(SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@RoleId", roles.SelectedRole);
            command.Parameters.AddWithValue("EmpoyeeId", id);

            await command.ExecuteNonQueryAsync();
        }

        return 1;
    }
}