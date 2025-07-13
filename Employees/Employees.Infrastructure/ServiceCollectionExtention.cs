using Employees.Domain;
using Employees.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Infrastructure;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        
        services.AddScoped<IEmployeesRepository, EmployeesRepository>();

        services.AddScoped<ICredentialsRepository, CredentialsRepository>();

        services.AddScoped<IAdditionalDetailsRepository, AdditionalDetailsRepository>();

        services.AddScoped<IFileRepository, FileRepository>();

        services.AddScoped<IEmploymentDetailsRepository, EmploymentDetailsRepository>();

        return services;
    }
}
