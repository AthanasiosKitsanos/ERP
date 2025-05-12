using ErpProject.Extentions;


namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCustomServices();

        builder.Services.AddCustomInterfaces();

        builder.Services.AddControllersWithViews();

        builder.Services.AddMemoryCache();

        builder.Services.AddAntiforgery();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.MapControllerRoute(name: "default", pattern: "{controller=LogIn}/{action=Index}");

        app.Run();
    }
}
