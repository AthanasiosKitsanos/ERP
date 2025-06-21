using Microsoft.Data.SqlClient;
using Employees.Contracts.EmployeeContracts;
using System.Data;

namespace Employees.Core.QueryBuilders;

public static class QueryBuilder
{
    public static (string query, List<SqlParameter> sqlParameters) UpdateDetailsQueryBuilder(this RequestEmployee.Update request, int id)
    {
        List<string> variables = new List<string>();
        List<SqlParameter> sqlParameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(request.Email))
        {
            variables.Add("Email = @Email");
            sqlParameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = request.Email });
        }

        if (!string.IsNullOrEmpty(request.Nationality))
        {
            variables.Add("Nationality = @Nationality");
            sqlParameters.Add(new SqlParameter("@Nationality", SqlDbType.NVarChar) { Value = request.Nationality });
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            variables.Add("PhoneNumber = @PhoneNumber");
            sqlParameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = request.PhoneNumber });
        }

        sqlParameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int) { Value = id });

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