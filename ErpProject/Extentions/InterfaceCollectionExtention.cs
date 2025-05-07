using ErpProject.Interfaces;
using ErpProject.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ErpProject.Extentions;

public static class InterfaceCollectionExtention
{
    public static IServiceCollection AddCustomInterfaces(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeServices>(sp => 
        {
            var realService = sp.GetRequiredService<EmployeeServices>();

            var cache  = sp.GetRequiredService<IMemoryCache>();

            var logger = sp.GetRequiredService<ILogger<CacheEmployeeServices>>();

            return new CacheEmployeeServices(realService, cache, logger);
        });

        return services;
    }
}
