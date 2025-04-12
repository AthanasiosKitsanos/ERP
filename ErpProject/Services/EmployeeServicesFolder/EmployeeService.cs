using System;
using ErpProject.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ErpProject.ContextDb;
using ErpProject.Helpers;
using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using ErpProject.Models.DTOModels;
using System.Transactions;
using ErpProject.Services.EmployeeServicesFolder;

namespace ErpProject.Services.EmployeeServices;

public class EmployeeService
{
    private readonly Connection _connection;
    private readonly AdditionalDetailsService _additionalDetails;

    public EmployeeService(Connection connection, AdditionalDetailsService additionalDetails)
    {
        _connection = connection;
        _additionalDetails = additionalDetails;
    }

    /// <summary>
    /// Asynchronously retrieves a list of employees from the database.
    /// </summary>
    /// <returns> Task that represents an asynchronous operation, which resolves to a list of Employees objects.</returns>
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        List<Employee> employeeList = new List<Employee>();

        string getEmployee = @"SELECT Id, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber 
                            FROM Employees";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(getEmployee, connection))
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
                        {"PhoneNumber", reader.GetOrdinal("PhoneNumber")}
                    };

                    while(await reader.ReadAsync())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(param["Id"]),
                            FirstName = reader.GetString(param["FirstName"]),
                            LastName = reader.GetString(param["LastName"]),
                            Email = reader.GetString(param["Email"]),
                            Age =  reader.GetString(param["Age"]),
                            DateOfBirth = reader.GetDateTime(param["DateOfBirth"]),
                            Nationality = reader.GetString(param["Nationality"]),
                            Gender = reader.GetString(param["Gender"]),
                            PhoneNumber = reader.GetString(param["PhoneNumber"])
                        };

                        employeeList.Add(employee);
                    }
                }
            }
        }

        return employeeList;
    }

    /// <summary>
    /// Asynchronously retrieves an Employee from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the Employee to retrieve.</param>
    /// <returns>Null if no Employee is found</returns>
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        Employee employee = new Employee();

        string getEmployee = @"SELECT Id, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber 
                            FROM Employees
                            WHERE Id = @Id";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(getEmployee, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        employee.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        employee.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        employee.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        employee.Email = reader.GetString(reader.GetOrdinal("Email"));
                        employee.Age = reader.GetString(reader.GetOrdinal("Age"));
                        employee.DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"));
                        employee.Nationality = reader.GetString(reader.GetOrdinal("Nationality"));
                        employee.Gender = reader.GetString(reader.GetOrdinal("Gender"));
                        employee.PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                    }
                }
            }
        }

        return employee;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if(string.IsNullOrEmpty(email))
        {
            return false;
        }

        string getEmail = @"SELECT COUNT(Email) 
                        FROM Employees 
                        WHERE Email = @Email";
        
        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command = new SqlCommand(getEmail, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());

                return count > 0;
            }
        }
    }

    /// <summary>
    /// Asynchronously deletes an Employee from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the Employee to retrieve.</param>
    /// <returns>True if deletion was a success, else False</returns>
    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        string delete = @"DELETE FROM Employees
                        WHERE Id = @Id";

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            using(SqlCommand command =  new SqlCommand(delete, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0;
            }
        }
    }

    /// <summary>
    /// Registers a new Employee to the Emloyee table
    /// </summary>
    /// <param name="employee">The EmployeeDTO class and Properties</param>
    /// <param name="connection">The connection string</param>
    /// <param name="transaction">The transaction</param>
    /// <returns>Returns the Id of the new added Employee</returns>
    public async Task<int> RegisterNewEmployeeAsync(EmployeeDTO employee, SqlConnection connection, SqlTransaction transaction)
    {
        if(employee is null)
        {
            return -1;
        }

        bool emailExists = await EmailExistsAsync(employee.Email);

        if(emailExists)
        {
            return -1;
        }
        
        string addEmployee = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, PhotographPath)
                            VALUES (@FirstName, @LastName, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @PhotographPath);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using(SqlCommand command = new SqlCommand(addEmployee, connection, transaction))
        {
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@Age", AgeCalculator.CalculateAge(employee.DateOfBirth));
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Nationality", employee.Nationality);
            command.Parameters.AddWithValue("@Gender", employee.Gender);
            command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
            command.Parameters.AddWithValue("@PhotographPath", employee.PhotographPath);

            var result = await command.ExecuteScalarAsync();

            return (result is not null) ? Convert.ToInt32(result) : -1;
        }
    }

    /// <summary>
    /// It completes the full registration of an employee
    /// </summary>
    /// <param name="model">A ViewModelDTO that uses all the Models of the DTO folder as properties</param>
    /// <returns>True if the transaction is completed, false if an error occures</returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> RegistrationCompleteAsync(ViewModelDTO model, IFormFile file)
    {
        if(model is null)
        {
            return false;
        }

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                int id = await RegisterNewEmployeeAsync(model.Employee, connection, transaction);

                await _additionalDetails.AddAdditionalDetailsAsync(model.AdditionalDetails, id, connection, transaction);

                await transaction.CommitAsync();
                return true;
            }
            catch(SqlException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Sql Error: {ex.Message}");
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
