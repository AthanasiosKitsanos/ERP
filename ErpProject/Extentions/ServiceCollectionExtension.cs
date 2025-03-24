using System;
using ErpProject.Data;
using ErpProject.Helpers.InitializeFolder;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<CreateFirstElements>();
        services.AddScoped<SeedData>();
        return services;
    }
}
