using Microsoft.Data.SqlClient;
using Employees.Domain.Models;
using System.Data;

namespace Employees.Shared.QueryBuilders;

public static class QueryBuilder
{
    public static (string query, List<SqlParameter> sqlParameters) UpdateDetailsQueryBuilder(this Employee employee)
    {
        List<string> variables = new List<string>();
        List<SqlParameter> sqlParameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(employee.Email))
        {
            variables.Add("Email = @Email");
            sqlParameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = employee.Email });
        }

        if (!string.IsNullOrEmpty(employee.Nationality))
        {
            variables.Add("Nationality = @Nationality");
            sqlParameters.Add(new SqlParameter("@Nationality", SqlDbType.NVarChar) { Value = employee.Nationality });
        }

        if (!string.IsNullOrEmpty(employee.PhoneNumber))
        {
            variables.Add("PhoneNumber = @PhoneNumber");
            sqlParameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = employee.PhoneNumber });
        }

        sqlParameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int) { Value = employee.Id });

        if (sqlParameters.Count == 0)
        {
            return (string.Empty, null!);
        }

        string query = $@"UPDATE Employees
                        SET {string.Join(", ", variables)}
                        WHERE EmployeeId = @EmployeeId";

        return (query, sqlParameters);
    }
}