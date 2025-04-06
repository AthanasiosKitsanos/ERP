using System;
using ErpProject.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ErpProject.ContextDb;
using ErpProject.Helpers;
using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using ErpProject.Models.DTOModels.Update;

namespace ErpProject.Services.EmployeeServices;

public class EmployeeService
{
    //private readonly ErpDbContext _dbContext;
    private readonly Connection _connection;

    public EmployeeService(/*ErpDbContext dbContext*/ Connection connection)
    {
        //_dbContext = dbContext;
        _connection = connection;
    }

    /// <summary>
    /// Registers a new Employee to the Employee Table
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>True, if registration was a success</returns>
    public async Task<bool> RegisterNewEmployeeAsync(Employee newEmployee)
    {
        if (newEmployee is null)
        {
            return false;
        }

        string emailQuery = @"SELECT COUNT(1) FROM Employees WHERE Email = @Email";

        string registerQuery = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, PhotographPath) " +
                       "VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @PhotographPath)";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(emailQuery, connection))
            {
                command.Parameters.AddWithValue("@Email", newEmployee.Email);

                int emailExistsCount = (int)(await command.ExecuteScalarAsync() ?? 0);

                if (emailExistsCount > 0)
                {
                    return false;
                }
            }

            using (SqlCommand command = new SqlCommand(registerQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", newEmployee.FirstName);
                command.Parameters.AddWithValue("@LastName", newEmployee.LastName);
                command.Parameters.AddWithValue("@Email", newEmployee.Email);
                command.Parameters.AddWithValue("@Age", AgeCalculator.CalculateAge(newEmployee.DateOfBirth));
                command.Parameters.AddWithValue("@DateOfBirth", newEmployee.DateOfBirth);
                command.Parameters.AddWithValue("@Nationality", newEmployee.Nationality);
                command.Parameters.AddWithValue("@Gender", newEmployee.Gender);
                command.Parameters.AddWithValue("@PhoneNumber", newEmployee.PhoneNumber);
                command.Parameters.AddWithValue("@PhotographPath", newEmployee.PhotographPath);

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    /// <summary>
    /// /// Gets all the elements from the Employee Table
    /// </summary>
    /// <returns>A List of the Employee in the Employee Table</returns>
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        List<Employee> employeeList = new List<Employee>();

        string query = @"SELECT Id, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber FROM Employees";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    Dictionary<string, int> collumnMap = new Dictionary<string, int>
                    {
                        {"Id", reader.GetOrdinal("Id")},
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
                        var employee = new Employee
                        {
                            Id = reader.GetInt32(collumnMap["Id"]),
                            FirstName = reader.GetString(collumnMap["FirstName"]),
                            LastName = reader.GetString(collumnMap["LastName"]),
                            Email = reader.GetString(collumnMap["Email"]),
                            Age = reader.GetString(collumnMap["Age"]),
                            DateOfBirth = reader.GetDateTime(collumnMap["DateOfBirth"]),
                            Nationality = reader.GetString(collumnMap["Nationality"]),
                            Gender = reader.GetString(collumnMap["Gender"]),
                            PhoneNumber = reader.GetString(collumnMap["PhoneNumber"])
                        };

                        employeeList.Add(employee);
                    }
                }
            }
        }

        return employeeList;
    }

    /// <summary>
    /// Gets an Employee element from the Employee Table
    /// </summary>
    /// <param name="id">Integer</param>
    /// <returns>An Employee class</returns>
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        Employee employee = null!;

        string query = @"SELECT Id, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber FROM Employees WHERE Id = @Id";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Age = reader.GetString(reader.GetOrdinal("Age")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                        };
                    }
                }
            }
        }

        return employee;
    }

    /// <summary>
    /// Updated the Email and PhoneNumber of an Employee
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns>True if at least one Row is affected</returns>
    public async Task<bool> UpdateEmployeeAsync(UpdateDTO dto, int id)
    {
        if (dto is null)
        {
            return false;
        }

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            List<string> fields = new List<string>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if(!string.IsNullOrEmpty(dto.Email))
            {
                fields.Add("Email = @Email");
                parameters["@Email"] = dto.Email;
            }

            if(!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                fields.Add("PhoneNumber = @PhoneNumber");
                parameters["@PhoneNumber"] = dto.PhoneNumber;
            }

            if(fields.Count == 0)
            {
                return false;
            }

            string query = $"UPDATE Employees SET {string.Join(", ", fields)} WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                foreach(var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Return the Id of the Employee</returns>
    /// <exception cref="ArgumentNullException">The email must not be null</exception>
    public async Task<int> GetIdFromEmployeeAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException("Email must not be empty or null", nameof(email));
        }

        int id = 0;

        string query = @"SELECT Id FROM Employees WHERE Email = @Email";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                }
            }
        }

        return id;
    }

    /// <summary>
    /// Deletes an Employee for the Database
    /// </summary>
    /// <param name="id">id (int)</param>
    /// <returns>True, if deletion is successfull</returns>
    public async Task<bool> DeleteEmployeeAsync(int id)
    {

        if (id <= 0)
        {
            return false;
        }

        string query = @"DELETE FROM Employees WHERE Id = @Id";

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }
}
