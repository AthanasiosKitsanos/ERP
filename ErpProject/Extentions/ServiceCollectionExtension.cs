using ErpProject.Helpers.Connection;
using ErpProject.Helpers;
using ErpProject.Services;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        services.AddScoped<EmployeeServices>();

        return services;
    }
}
