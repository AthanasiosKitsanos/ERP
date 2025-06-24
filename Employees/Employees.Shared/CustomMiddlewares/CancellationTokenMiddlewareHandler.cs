using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Employees.Shared.CustomMiddlewares;

public class CancellationTokenMiddlewareHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CancellationTokenMiddlewareHandler> _logger;

    public CancellationTokenMiddlewareHandler(RequestDelegate next, ILogger<CancellationTokenMiddlewareHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvoleAsync(HttpContext context)
    {
        CancellationToken token = context.RequestAborted;

        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation cancelled by the user!");
            
            context.Response.StatusCode = 499;
        }
        catch (Exception)
        {
            _logger.LogWarning("There was an error during the middleware runtime!");
            throw;
        }
    }
}
