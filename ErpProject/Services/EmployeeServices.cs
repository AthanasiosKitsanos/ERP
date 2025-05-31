using System.Data;
using ErpProject.Helpers;
using ErpProject.Helpers.Connection;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class EmployeeServices
{
    private readonly Connection _connection;

    public EmployeeServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        string query = @"SELECT COUNT(*)
                        FROM Employees
                        WHERE Email = @Email";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
        }
    }

    public async Task<int> AddEmployeeAsync(Employee employee)
    {
        if (employee is null || employee.PhotoFile is null)
        {
            return 0;
        }

        employee.MIME = employee.PhotoFile.ContentType;

        await using (MemoryStream memoryStream = new MemoryStream())
        {
            await employee.PhotoFile.CopyToAsync(memoryStream);

            employee.Photograph = memoryStream.ToArray();
        }

        string query = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Photograph, MIME, CreatedAt)
                        VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @Photograph, @MIME, @CreatedAt);
                        
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = employee.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = employee.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = employee.Email;
                command.Parameters.Add("@Age", SqlDbType.NVarChar).Value = AgeCalculator.CalculateAge(employee.DateOfBirth);
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = employee.DateOfBirth;
                command.Parameters.Add("@Nationality", SqlDbType.NVarChar).Value = employee.Nationality;
                command.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = employee.Gender;
                command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = employee.PhoneNumber;
                command.Parameters.Add("@Photograph", SqlDbType.VarBinary).Value = employee.Photograph;
                command.Parameters.Add("@MIME", SqlDbType.NVarChar).Value = employee.MIME;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = DateTime.Now;

                int id = Convert.ToInt32(await command.ExecuteScalarAsync());

                return id;
            }
        }
    }

    public async IAsyncEnumerable<Employee> GetAllEmployeesAsync()
    {
        string query = @"SELECT EmployeeId, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber
                        FROM Employees
                        WHERE IsCompleted = 1";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> param = new Dictionary<string, int>
                    {
                        {"EmployeeId", reader.GetOrdinal("EmployeeId")},
                        {"FirstName", reader.GetOrdinal("FirstName")},
                        {"LastName", reader.GetOrdinal("LastName")},
                        {"Email", reader.GetOrdinal("Email")},
                        {"Age", reader.GetOrdinal("Age")},
                        {"DateOfBirth", reader.GetOrdinal("DateOfBirth")},
                        {"Nationality", reader.GetOrdinal("Nationality")},
                        {"Gender", reader.GetOrdinal("Gender")},
                        {"PhoneNumber", reader.GetOrdinal("PhoneNumber")}
                    };

                    while (await reader.ReadAsync())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(param["EmployeeId"]),
                            FirstName = reader.GetString(param["FirstName"]),
                            LastName = reader.GetString(param["LastName"]),
                            Email = reader.GetString(param["Email"]),
                            Age = reader.GetString(param["Age"]),
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(param["DateOfBirth"])),
                            Nationality = reader.GetString(param["Nationality"]),
                            Gender = reader.GetString(param["Gender"]),
                            PhoneNumber = reader.GetString(param["PhoneNumber"])
                        };

                        yield return employee;
                    }
                }
            }
        }
    }

    public async Task<bool> DeleteEmployeeByIdAsync(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        string query = @"DELETE FROM Employees 
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null!;
        }

        Employee employee = new Employee();

        string query = @"SELECT EmployeeId, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber
                        FROM Employees
                        WHERE EmployeeId = @EmployeeId AND IsCompleted = 1";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> param = new Dictionary<string, int>
                    {
                        {"EmployeeId", reader.GetOrdinal("EmployeeId")},
                        {"FirstName", reader.GetOrdinal("FirstName")},
                        {"LastName", reader.GetOrdinal("LastName")},
                        {"Email", reader.GetOrdinal("Email")},
                        {"Age", reader.GetOrdinal("Age")},
                        {"DateOfBirth", reader.GetOrdinal("DateOfBirth")},
                        {"Nationality", reader.GetOrdinal("Nationality")},
                        {"Gender", reader.GetOrdinal("Gender")},
                        {"PhoneNumber", reader.GetOrdinal("PhoneNumber")}
                    };

                    if (await reader.ReadAsync())
                    {
                        employee.Id = reader.GetInt32(param["EmployeeId"]);
                        employee.FirstName = reader.GetString(param["FirstName"]);
                        employee.LastName = reader.GetString(param["LastName"]);
                        employee.Email = reader.GetString(param["Email"]);
                        employee.Age = reader.GetString(param["Age"]);
                        employee.DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(param["DateOfBirth"]));
                        employee.Nationality = reader.GetString(param["Nationality"]);
                        employee.Gender = reader.GetString(param["Gender"]);
                        employee.PhoneNumber = reader.GetString(param["PhoneNumber"]);
                    }
                }
            }

            return employee;
        }
    }

    public async Task<bool> EditEmployeeAsync(int id, Employee employee)
    {
        if (id <= 0)
        {
            return false;
        }

        List<string> additions = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(employee.Email) || !string.IsNullOrWhiteSpace(employee.Email))
        {
            additions.Add("Email = @Email");
            parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = employee.Email });
        }

        if (!string.IsNullOrEmpty(employee.PhoneNumber) || !string.IsNullOrWhiteSpace(employee.PhoneNumber))
        {
            additions.Add("PhoneNumber = @PhoneNumber");
            parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = employee.PhoneNumber });
        }

        string query = $@"UPDATE Employees
                        SET {string.Join(", ", additions)}
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.AddRange(parameters.ToArray());

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }
}