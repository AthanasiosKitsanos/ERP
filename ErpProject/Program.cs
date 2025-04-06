using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using System.Threading.Tasks;
using ErpProject.Extentions;


namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ErpDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCustomServices();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
