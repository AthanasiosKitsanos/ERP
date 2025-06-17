using Employees.Core.IServices;
using Employees.Core.Services;
using Employees.Domain;
using Employees.Infrastructure.IRepository;
using Employees.Infrastructure.Repository;

namespace Employees.Api.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddAntiforgery();

        //services.AddAuthentication("ErpJwt").AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("ErpJwt", options => {});

        services.AddAuthorization();
        
        
        return services;
    }
}
