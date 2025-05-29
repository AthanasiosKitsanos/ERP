using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly JWTDemoKey _key;
    public JwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, TimeProvider timeProvider, JWTDemoKey key) : base(options, logger, encoder, new SystemClock())
    {
        _timeProvider = timeProvider;
        _key = key;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string token = string.Empty;

        if (Request.Headers.TryGetValue("Authorization", out var authHeader) && authHeader.ToString().StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            token = authHeader.ToString().Substring("Bearer".Length).Trim();
        }
        else
        {
            Request.Cookies.TryGetValue("jwt", out token!);
        }

        if (string.IsNullOrEmpty(token))
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        string[] parts = token.Split('.');

        string encodedHeader = parts[0];
        string encodedPayload = parts[1];


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
        string payloadJson = Encoding.UTF8.GetString(payloadBytes);

        JWTHeader header = HeaderDeserializer(headerJson);

        if (header is null)
        {
            Logger.LogWarning("Unsupported or missing JWT algorithm");
            return await Task.FromResult(AuthenticateResult.Fail("Unsupported JWT algorithm"));
        }

        string encodedSignature = parts[2];

        string unsignedToken = $"{encodedHeader}.{encodedPayload}";

        string secretKey = _key.DemonstrationKey;

        bool result = ValidateSignature(encodedSignature, unsignedToken, secretKey);

        if (!result)
        {
            Logger.LogWarning("JWT Signature validation failed");
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Signature"));
        }

        JWTPayload payload = PayloadDeserializer(payloadJson);

        if (payload is null || string.IsNullOrEmpty(payload.Role))
        {
            return await Task.FromResult(AuthenticateResult.Fail("Missing payload or there is no role"));
        }

        long expirationUnixTime = payload.Expiration;
        DateTimeOffset expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationUnixTime);
        DateTimeOffset timeNow = _timeProvider.GetUtcNow();

        if (expirationDate < timeNow)
        {
            Logger.LogInformation("JWT token has expired");
            return await Task.FromResult(AuthenticateResult.Fail("Token expired"));
        }

        List<Claim> claims = CreateClaims(payload);

        ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties? properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.ContentType = "application/json";

        string message = "Authentication falied. Token is missing, invalid or expired";

        string result = JsonSerializer.Serialize(new
        {
            error = message,
            scheme = Scheme.Name
        });

        return Response.WriteAsync(result);
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

    private static bool ValidateSignature(string encodedSignature, string unsignedToken, string secretKey)
    {
        byte[] expectedSignature = Sign(unsignedToken, secretKey);

        byte[] providedSignature = Base64UrlDecode(encodedSignature);

        if (!CryptographicOperations.FixedTimeEquals(providedSignature, expectedSignature))
        {
            return false;
        }

        return true;

    }
    private static JWTHeader HeaderDeserializer(string headerJson)
    {
        JWTHeader header =JsonSerializer.Deserialize<JWTHeader>(headerJson)!;

        if (header is null || !string.Equals(header.algorithm, "HS256", StringComparison.OrdinalIgnoreCase))
        {
            return null!;
        }

        return header;
    }

    private static JWTPayload PayloadDeserializer(string payloadJson)
    {
        JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJson)!;
        
        if (payload is null)
        {
            return null!;
        }

        return payload;
    }

    private static List<Claim> CreateClaims(JWTPayload payload)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, payload.Audience),
            new Claim("UserId", payload.Id.ToString()),
            new Claim(ClaimTypes.GivenName, payload.FirstName),
            new Claim(ClaimTypes.Surname, payload.LastName),
            new Claim(ClaimTypes.Role, payload.Role),
            new Claim("token_validity_seconds", payload.TokenValidityOnSeconds.ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, payload.Expiration.ToString())
        };

        return claims;
    }
}