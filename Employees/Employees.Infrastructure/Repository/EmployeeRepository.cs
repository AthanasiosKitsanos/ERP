using System.Data;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Employees.Shared;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly Connection _connection;

    public EmployeeRepository(Connection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(Employee employee)
    {
        string query = @"INSERT INTO Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Mime, Photograph, CreatedAt)
                        VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @Mime, @Photograph, @CreatedAt);
                        
                        SELECT CAST(SCOPED_IDENTITY() AS INT)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = employee.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = employee.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = employee.Email;
                command.Parameters.Add("@Age", SqlDbType.NVarChar).Value = employee.DateOfBirth.CalculateAge();
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = employee.DateOfBirth;
                command.Parameters.Add("@Nationality", SqlDbType.NVarChar).Value = employee.Nationality;
                command.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = employee.Gender;
                command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = employee.PhoneNumber;
                command.Parameters.Add("@Mime", SqlDbType.NVarChar).Value = employee.PhotoFile!.ContentType;
                command.Parameters.Add("@Photograph", SqlDbType.VarBinary).Value = employee.Photograph.GetArrayOfBytes(employee.PhotoFile!);
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = DateTime.Now;

                return Convert.ToInt32(await command.ExecuteScalarAsync()); 
            }
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await Task.Run(() => true);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Task.Run(() => true);
    }

    public async IAsyncEnumerable<Employee> GetAllAsync()
    {
        yield return await Task.Run( () => new Employee());
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        return await Task.Run(() => new Employee());
    }

    public async Task<bool> UpdateAsync(int id, Employee employee)
    {
        return await Task.Run(() => true);
    }
}
