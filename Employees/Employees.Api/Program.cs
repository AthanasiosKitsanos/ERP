using Employees.Api.Extentions;
using Employees.Core;
using Employees.Infrastructure;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddJsonFiles();
        
        builder.Services.AddServices();
        builder.Services.AddCore();
        builder.Services.AddInfrastructure();
        
        var app = builder.Build();

        app.AddMiddleware();

        app.Run();
    }
}