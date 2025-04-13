using ErpProject.Models.RolesModel;
using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;

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
        List<Roles> roleList = new List<Roles>();

        string query = @"SELECT RoleName 
                        FROM Roles";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();
            
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> collumnMap = new Dictionary<string, int>
                    {
                        {"RoleName", reader.GetOrdinal("RoleName")}
                    };

                    while(await reader.ReadAsync())
                    {
                        Roles role = new Roles
                        {
                            RoleName = reader.GetString(collumnMap["RoleName"])
                        };

                        roleList.Add(role);
                    }
                }
            }
        }

        return roleList;
    }

     /// <summary>
    /// Adds a role to the Employee
    /// </summary>
    /// <param name="employee">Param for an Employee object</param>
    /// <param name="roleName">The role's name</param>
    /// <returns>True of the role is added. else false</returns>
    public async Task<bool> AddRoleToEmployeeAsync(int id, string roleName)
    {
        if(id <=0 || string.IsNullOrEmpty(roleName))
        {
            return false;
        }

        int roleId = 0;

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            string getRoleId = @"SELECT Id 
                                FROM Roles 
                                WHERE RoleName = @RoleName";

            using(SqlCommand command = new SqlCommand(getRoleId, connection))
            {
                command.Parameters.AddWithValue("@RoleName", roleName);

                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        roleId = reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                }
            }

            string addRole = @"INSERT INTO RoleEmployee (RoleId, EmployeeId) 
                            VALUES (@RoleId, @EmployeeId)";

            using(SqlCommand command = new SqlCommand(addRole, connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);
                command.Parameters.AddWithValue("@EmployeeId", id);

                await command.ExecuteNonQueryAsync();

                return true;
            }   
        } 
    }
}