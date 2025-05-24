using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ErpProject.JsonWebToken;

public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly TimeProvider _timeProvider;
    public JwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, TimeProvider timeProvider) : base(options, logger, encoder, new SystemClock())
    {
        _timeProvider = timeProvider;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        DateTimeOffset timeNow = _timeProvider.GetUtcNow();

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            Logger.LogDebug("No Authorization header was found");
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        string authHeader = Request.Headers["Authorization"].ToString();

        if (!authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            Logger.LogWarning("Authorization header is not Bearer");
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        string token = authHeader.Substring("Bearer".Length).Trim();

        string[] parts = token.Split('.');

        string encodedHeader = parts[0];
        string encodedPayload = parts[1];
        string ecnodedSignature = parts[2];

        byte[] headerBytes;
        byte[] payloadBytes;

        try
        {
            headerBytes = Base64UrlDecode(encodedHeader);
            payloadBytes = Base64UrlDecode(encodedPayload);
        }
        catch (FormatException)
        {
            Logger.LogError("Failed to Base64Url-decode header or payload");
            return await Task.FromResult(AuthenticateResult.Fail("Invalid token encoding"));
        }

        string headerJson = Encoding.UTF8.GetString(headerBytes);
        string payloadJason = Encoding.UTF8.GetString(payloadBytes);

        //TODO: The rest of the jwt reading

        return await Task.FromResult(AuthenticateResult.Fail("Not implemented"));
    }

    private static byte[] Sign(string unsignedToken, string secretKey)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

        using var hmac = new HMACSHA256(secretKeyBytes);

        return hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
    }

    private static byte[] Base64UrlDecode(string input)
    {
        string padded = input.Replace('-', '+').Replace('_', '/');

        switch (padded.Length % 4)
        {
            case 2:
                padded += "==";
                break;

            case 3:
                padded += "=";
                break;
        }

        return Convert.FromBase64String(padded);
    }
}