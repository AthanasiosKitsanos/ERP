using Employees.Domain;

namespace Employees.Api.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        
        return services;
    }
}
