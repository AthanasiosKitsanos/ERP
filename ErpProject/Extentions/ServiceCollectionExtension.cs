using System;
using ErpProject.Data;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<SeedData>();
        return services;
    }
}
