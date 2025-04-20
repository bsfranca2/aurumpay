using System.Security.Cryptography;
using System.Text;

using AurumPay.Checkout.Api.Infrastructure.Options;
using AurumPay.Domain.Interfaces;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class RequestDeviceIdentityService(
    IOptions<SecurityOptions> securityOptions,
    IHttpContextAccessor httpContextAccessor
) : IDeviceIdentityProvider
{
    private readonly string _salt = !string.IsNullOrEmpty(securityOptions.Value.FingerprintSalt)
        ? securityOptions.Value.FingerprintSalt
        : throw new InvalidOperationException("Fingerprint salt must be configured in security options.");

    private string? _fingerprintCached;

    public string GetFingerprint()
    {
        if (_fingerprintCached == null)
        {
            _fingerprintCached = GenerateFingerprint(httpContextAccessor.HttpContext!);
        }

        return _fingerprintCached;
    }

    private string GenerateFingerprint(HttpContext? httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        StringBuilder sb = new();

        string ipAddress = GetClientIpAddress(httpContext);
        sb.Append($"ip:{ipAddress};");

        string userAgent = httpContext.Request.Headers.UserAgent.ToString();
        sb.Append($"ua:{userAgent};");

        string acceptLanguage = httpContext.Request.Headers.AcceptLanguage.ToString();
        sb.Append($"lang:{acceptLanguage};");

        string host = httpContext.Request.Headers.Host.ToString();
        sb.Append($"host:{host};");

        if (httpContext.Request.Headers.TryGetValue("sec-ch-ua", out StringValues browserInfo))
        {
            sb.Append($"browser:{browserInfo};");
        }

        if (httpContext.Request.Headers.TryGetValue("sec-ch-ua-platform", out StringValues platform))
        {
            sb.Append($"platform:{platform};");
        }

        sb.Append($"salt:{_salt}");

        return ComputeSha256Hash(sb.ToString());
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        string ip = string.Empty;

        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues forwardedFor))
        {
            ip = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim() ?? string.Empty;
        }

        if (string.IsNullOrEmpty(ip) && context.Request.Headers.TryGetValue("X-Real-IP", out StringValues realIp))
        {
            ip = realIp.ToString();
        }

        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        return ip;
    }

    private static string ComputeSha256Hash(string rawData)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));

        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}