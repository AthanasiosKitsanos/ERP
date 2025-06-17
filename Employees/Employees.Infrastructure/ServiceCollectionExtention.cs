using Employees.Infrastructure.IRepository;
using Employees.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Infrastructure;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEmployeesRepository, EmployeesRepository>();

        return services;
    }
}
