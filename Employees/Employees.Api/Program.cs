using Employees.Api.Extentions;
using Employees.Core;
using Employees.Infrastructure;
using Employees.BackgroundServices;
using Employees.RazorComponents;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddJsonFiles();

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "[HH:mm:ss]";
        });
        builder.Services.AddServices();
        builder.Services.AddBackgroundServices();
        builder.Services.AddCore();
        builder.Services.AddInfrastructure();
        
        var app = builder.Build();

        app.AddMiddleware();

        app.Run();
    }
}