using Employees.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Employees.Shared.Validators;

public static class EmployeeValidator
{
    public static bool IsNull(this Employee employee)
    {
        if (employee is null)
        {
            return true;
        }

        return false;
    }

    public static bool IsNullOrLessThanZero(this int id)
    {
        if (id <= 0)
        {
            return true;
        }

        return false;
    }

    public static bool IsEmptyOrNull( this (string query, List<SqlParameter> sqlParameters) objects)
    {
        if (string.IsNullOrEmpty(objects.query) || objects.sqlParameters is null)
        {
            return true;
        }

        return false;
    }
}
