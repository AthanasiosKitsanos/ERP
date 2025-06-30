using Microsoft.Extensions.DependencyInjection;

namespace Employees.RazorComponents;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddComponentServices(this IServiceCollection services)
    {
        services.AddScoped<AntiForgeryServices>();

        return services;
    }
}
