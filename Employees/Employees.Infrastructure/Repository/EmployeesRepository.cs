using System.Data;
using System.Runtime.CompilerServices;
using Employees.Domain;
using Employees.Domain.Models;
using Employees.Infrastructure.IRepository;
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
                        
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = employee.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = employee.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = employee.Email;
                command.Parameters.Add("@Age", SqlDbType.NVarChar).Value = employee.Age;
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = employee.DateOfBirth;
                command.Parameters.Add("@Nationality", SqlDbType.NVarChar).Value = employee.Nationality;
                command.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = employee.Gender;
                command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = employee.PhoneNumber;
                command.Parameters.Add("@Mime", SqlDbType.NVarChar).Value = employee.MIME;
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

        string query = @"SELECT EmployeeId, FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber
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

        return null!;
    }

    public async Task<bool> UpdateAsync(string query, List<SqlParameter> parameters, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters.ToArray());

                cancellationToken.ThrowIfCancellationRequested();

                return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
            }
        }
    }

    public async Task<Employee> GetInfoForDeleteAysnc(int id, CancellationToken token = default)
    {
        string query = @"SELECT EmployeeId, FirstName, LastName
                        FROM Employees
                        WHERE EmployeeId = @EmployeeId";

        await using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync(token);

            await using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

                await using (SqlDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    if (await reader.ReadAsync(token))
                    {
                        return new Employee
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        };
                    }
                }
            }
        }

        return null!;
    }
}
