using Employees.Core.IServices;
using Employees.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Core;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IEmployeesServices, EmployeesServices>();

        services.AddScoped<ICredentialsServices, CredentialsServices>();

        //services.AddScoped<IEmployeesValidators, EmployeesValidators>();

        return services;
    }
}
