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
            return AuthenticateResult.NoResult();
        }

        string authorizationHeader = Request.Headers["Authorization"]!;

        if (string.IsNullOrEmpty("authorizationHeader") || !authorizationHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Invalide Authorazation Format");
        }

        string token = authorizationHeader.Substring("Bearer".Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Token is missing");
        }

        string[] tokenParts = token.Split(".");

        if (tokenParts.Length != 3)
        {
            return AuthenticateResult.Fail("The token must consist of a header, payload and a signature");
        }

        string headerJson = Base64UrlDecode(tokenParts[0]);
        string payloadJson = Base64UrlDecode(tokenParts[1]);
        string signature = tokenParts[2];

        var header = JsonSerializer.Deserialize<Dictionary<string, object>>(headerJson);
        var payload = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);

        return AuthenticateResult.Fail("Not implemented");
    }

    private static string Base64UrlDecode(string input)
    {
        string base64 = input.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "==";
                break;

            case 3: base64 += "=";
                break;
        }

        byte[] bytes = Convert.FromBase64String(base64);

        return Encoding.UTF8.GetString(bytes);
    }
}