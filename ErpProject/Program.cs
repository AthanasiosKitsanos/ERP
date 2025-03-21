using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using System.Threading.Tasks;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ErpDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        using(var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
            
            var active = new AccountStatus { StatusName = "Active" };

            var inactive = new AccountStatus { StatusName = "Inactive" };

            var blocked = new AccountStatus { StatusName = "Blocked" };

            context.AccountStatus.Add(active);
            context.AccountStatus.Add(inactive);
            context.AccountStatus.Add(blocked);
            context.SaveChanges();
        }

        app.Run();
    }
}
