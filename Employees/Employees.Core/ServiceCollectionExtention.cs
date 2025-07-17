using Employees.Core.Services;
using Employees.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Core;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IEmployeesServices, EmployeesServices>();

        services.AddScoped<ICredentialsServices, CredentialsServices>();

        services.AddScoped<IAdditionalDetailsServices, AdditionalDetailsServices>();

        services.AddScoped<IFileServices, FileServices>();

        services.AddScoped<IEmploymentDetailsServices, EmploymentDetailsServices>();

        services.AddScoped<IRolesServices, RolesServices>();

        return services;
    }
}
