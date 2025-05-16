using ErpProject.Interfaces;
using ErpProject.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ErpProject.Extentions;

public static class InterfaceCollectionExtention
{
    public static IServiceCollection AddCustomInterfaces(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeServices, EmployeeServices>();

        return services;
    }
}
