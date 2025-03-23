using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using System.Threading.Tasks;
using ErpProject.Extentions;
using ErpProject.Data;


namespace MainProgram;

class MainProgram
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ErpDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCustomServices();

        var app = builder.Build();

        using(var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var newAdditions = services.GetRequiredService<SeedData>();
            
            await newAdditions.InitializeAsync();
        }

        app.Run();
    }
}
