using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
using Employees.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace Employees.Infrastructure.Repository;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly Connection _connection;

    public EmployeesRepository(Connection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string query = @"INSERT INTO dbo.Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Mime, Photograph, CreatedAt)
                        VALUES (@FirstName, @LastName, @Email, @Age, @DateOfBirth, @Nationality, @Gender, @PhoneNumber, @Mime, @Photograph, @CreatedAt);
                        
                        SELECT CAST(SCOPED_IDENTITY() AS INT)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

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
                command.Parameters.Add("@Photograph", SqlDbType.VarBinary).Value = employee.Photograph;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = DateTime.Now;

                cancellationToken.ThrowIfCancellationRequested();

                return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
            }
        }
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string query = @"DELETE FROM dbo.Employees
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                cancellationToken.ThrowIfCancellationRequested();

                return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
            }
        }
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string query = @"SELECT COUNT(*)
                        FROM dbo.Employees
                        WHERE Email = @Email";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                cancellationToken.ThrowIfCancellationRequested();

                return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) > 0;
            }
        }
    }

    public async IAsyncEnumerable<Employee> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string query = @"SELECT EmployeeId, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber
                        FROM dbo.Employees";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                await using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        
                        yield return new Employee
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            Age = reader.GetString(4),
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(5)),
                            Nationality = reader.GetString(6),
                            Gender = reader.GetString(7),
                            PhoneNumber = reader.GetString(8)
                        };
                    }
                }
            }
        }
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string query = @"SELECT FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, Mime, Photograph
                        FROM dbo.Employees
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        
                        return new Employee
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Email = reader.GetString(2),
                            Age = reader.GetString(3),
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(4)),
                            Nationality = reader.GetString(5),
                            Gender = reader.GetString(6),
                            PhoneNumber = reader.GetString(7),
                            MIME = reader.GetString(8),
                            Photograph = await reader.GetFieldValueAsync<byte[]>(9)
                        };
                    }
                }
            }
        }

        return null!;
    }

    public async Task<bool> UpdateAsync(int id, Employee employee, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<string> variables = new List<string>();
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(employee.Email))
        {
            variables.Add("Email = @Email");
            parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = employee.Email });
        }

        if (!string.IsNullOrEmpty(employee.PhoneNumber))
        {
            variables.Add("PhoneNumber = @PhoneNumber");
            parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = employee.PhoneNumber });
        }

        if (!string.IsNullOrEmpty(employee.Nationality))
        {
            variables.Add("Nationality = @Nationality");
            parameters.Add(new SqlParameter("@Nationality", SqlDbType.NVarChar) { Value = employee.Nationality });
        }

        if (employee.PhotoFile is not null)
        {
            variables.Add("Photograph = @Photograph, Mime = @Mime");
            parameters.Add(new SqlParameter("@Mime", SqlDbType.NVarChar) { Value = employee.PhotoFile.ContentType });
            parameters.Add(new SqlParameter(@"Photograph", SqlDbType.VarBinary) { Value = employee.Photograph });
        }

        if (variables.Count == 0)
        {
            return false;
        }

        string query = @$"UPDATE dbo.Employees
                        SET {string.Join(", ", variables)}
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                command.Parameters.AddRange(parameters.ToArray());

                cancellationToken.ThrowIfCancellationRequested();

                return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
            }
        }
    }
}
