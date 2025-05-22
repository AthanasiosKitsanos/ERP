using System.Security.Cryptography;
using System.Text;
using ErpProject.Models;

namespace ErpProject.JsonWebToken;

public class JWTServices
{
    private readonly JWTHeader _header;
    private readonly JWTDemoKey _key;

    public JWTServices(JWTHeader header, JWTDemoKey key)
    {
        _header = header;
        _key = key;
    }

    public string CreateJWToken(LoggedInData data, bool rememberMe)
    {
        JWTPayload payload = new JWTPayload(data, rememberMe);

        string payloadJson = payload.Serialize();

        string headerJson = _header.Serialize();

        return BuildJWToken(headerJson, payloadJson, _key);
    }

    private static string BuildJWToken(string headerJson, string payloadJson, JWTDemoKey key)
    {
        string encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        string encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

        string unsignedToken = $"{encodedHeader}.{encodedPayload}";

        byte[] signatureBytes = Sign(unsignedToken, key.DemonstrationKey);

        string encodedSignature = Base64UrlEncode(signatureBytes);

        return $"{unsignedToken}.{encodedSignature}";
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static byte[] Sign(string unsignedToken, string secretKey)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

        using var hmac = new HMACSHA256(secretKeyBytes);

        return hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
    }
}
