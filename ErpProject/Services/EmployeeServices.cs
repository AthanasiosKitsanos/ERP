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

    public async Task<bool> AddEmployeeAsync(Employee employee)
    {
        if(employee is null || employee.PhotoFile is null)
        {
            return false;
        }

        employee.MIME = employee.PhotoFile.ContentType;

        using(MemoryStream memoryStream = new MemoryStream())
        {
            await employee.PhotoFile.CopyToAsync(memoryStream);

            employee.Photograph = memoryStream.ToArray();
        }

        string query = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Photograph, MIME)
                        VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @Photograph, @MIME)";

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

                int affectedRows = await command.ExecuteNonQueryAsync();
                
                return affectedRows > 0;
            }
        }
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        List<Employee> employeeList = new List<Employee>();

        string query = @"SELECT *
                        FROM Employees";

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
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(param["dateOfBirth"])),
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
}
