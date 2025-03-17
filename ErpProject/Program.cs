using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ErpDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        app.Run();
    }
}
