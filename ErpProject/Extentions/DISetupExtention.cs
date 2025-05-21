namespace ErpProject.Extentions;

public static class DISetupExtention
{
    public static IServiceCollection AddDISetup(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddAntiforgery();

        services.AddAuthentication();

        services.AddAuthorization();

        return services;
    }
}
