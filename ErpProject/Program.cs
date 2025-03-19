using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using System.Threading.Tasks;

namespace MainProgram;

class MainProgram
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ErpDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        using(var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
            var adminRole = new Roles { RoleName = "Admin" };
            var managerRole = new Roles { RoleName = "Manager" };
            var userRole = new Roles { RoleName = "User" };

            context.Roles.Add(adminRole);
            context.Roles.Add(managerRole);
            context.Roles.Add(userRole);
            context.SaveChanges();
        }

        app.Run();
    }
}
