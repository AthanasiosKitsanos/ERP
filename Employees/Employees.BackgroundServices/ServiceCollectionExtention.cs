using Microsoft.Extensions.DependencyInjection;

namespace Employees.BackgroundServices;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        // services.AddHostedService<RefreshTokenCleanUpService>();
        // services.AddHostedService<RegistrationCleanUpService>();

        return services;
    }
}
