using System.Data;
using ErpProject.Helpers;
using ErpProject.Helpers.Connection;
using ErpProject.Interfaces;
using ErpProject.Models;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services;

public class EmployeeServices: IEmployeeServices
{
    private readonly Connection _connection;

    public EmployeeServices(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if(string.IsNullOrEmpty(email))
        {
            return false;
        }

        string query = @"SELECT COUNT(*)
                        FROM Employees
                        WHERE Email = @Email";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
        }
    }

    public async Task<int> AddEmployeeAsync(Employee employee)
    {
        if(employee is null || employee.PhotoFile is null)
        {
            return 0;
        }

        employee.MIME = employee.PhotoFile.ContentType;

        using(MemoryStream memoryStream = new MemoryStream())
        {
            await employee.PhotoFile.CopyToAsync(memoryStream);

            employee.Photograph = memoryStream.ToArray();
        }

        string query = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Photograph, MIME, CreatedAt)
                        VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @Photograph, @MIME, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
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

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        List<Employee> employeeList = new List<Employee>();

        string query = @"SELECT *
                        FROM Employees
                        WHERE IsCompleted = 1";

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
                        {"FirstName", reader.GetOrdinal("FirstName")},
                        {"LastName", reader.GetOrdinal("LastName")},
                        {"Email", reader.GetOrdinal("Email")},
                        {"Age", reader.GetOrdinal("Age")},
                        {"DateOfBirth", reader.GetOrdinal("DateOfBirth")},
                        {"Nationality", reader.GetOrdinal("Nationality")},
                        {"Gender", reader.GetOrdinal("Gender")},
                        {"PhoneNumber", reader.GetOrdinal("PhoneNumber")},
                        {"Photograph", reader.GetOrdinal("Photograph")},
                        {"MIME", reader.GetOrdinal("MIME")}
                    };

                    while(await reader.ReadAsync())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(param["Id"]),
                            FirstName = reader.GetString(param["FirstName"]),
                            LastName = reader.GetString(param["LastName"]),
                            Email = reader.GetString(param["Email"]),
                            Age = reader.GetString(param["Age"]),
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(param["DateOfBirth"])),
                            Nationality = reader.GetString(param["Nationality"]),
                            Gender = reader.GetString(param["Gender"]),
                            PhoneNumber = reader.GetString(param["PhoneNumber"]),
                            Photograph = await reader.GetFieldValueAsync<byte[]>(param["Photograph"]),
                            MIME = reader.GetString(param["MIME"])
                        };

                        employeeList.Add(employee);
                    }
                }
            }
        }

        return employeeList;
    }

    public async Task<bool> DeleteEmployeeByIdAsync(int id)
    {
        if(id <= 0)
        {
            return false;
        }

        string query = @"DELETE FROM Employees 
                        WHERE Id = @Id";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        if(id <= 0)
        {
            return null!;
        }
        
        Employee employee = new Employee();
        string query = @"SELECT *
                        FROM Employees
                        WHERE Id = @Id AND IsCompleted = 1";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> param = new Dictionary<string, int>
                    {
                        {"Id", reader.GetOrdinal("Id")},
                        {"FirstName", reader.GetOrdinal("FirstName")},
                        {"LastName", reader.GetOrdinal("LastName")},
                        {"Email", reader.GetOrdinal("Email")},
                        {"Age", reader.GetOrdinal("Age")},
                        {"DateOfBirth", reader.GetOrdinal("DateOfBirth")},
                        {"Nationality", reader.GetOrdinal("Nationality")},
                        {"Gender", reader.GetOrdinal("Gender")},
                        {"PhoneNumber", reader.GetOrdinal("PhoneNumber")},
                        {"Photograph", reader.GetOrdinal("Photograph")},
                        {"MIME", reader.GetOrdinal("MIME")}
                    };

                    if(await reader.ReadAsync())
                    {
                        employee.Id = reader.GetInt32(param["Id"]);
                        employee.FirstName = reader.GetString(param["FirstName"]);
                        employee.LastName = reader.GetString(param["LastName"]);
                        employee.Email = reader.GetString(param["Email"]);
                        employee.Age = reader.GetString(param["Age"]);
                        employee.DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(param["DateOfBirth"]));
                        employee.Nationality = reader.GetString(param["Nationality"]);
                        employee.Gender = reader.GetString(param["Gender"]);
                        employee.PhoneNumber = reader.GetString(param["PhoneNumber"]);
                        employee.Photograph = await reader.GetFieldValueAsync<byte[]>(param["Photograph"]);
                        employee.MIME = reader.GetString(param["MIME"]);
                    }
                }
            }

            return employee;
        }
    }
}
