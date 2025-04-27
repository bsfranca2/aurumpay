using System.Net;

using Microsoft.AspNetCore.HttpOverrides;

using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace AurumPay.Checkout.Api.Infrastructure.Extensions;

public static class ForwardedHeadersExtensions
{
    public static IServiceCollection AddForwardedHeadersConfig(this IServiceCollection services, IHostEnvironment env,
        IConfiguration configuration)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost |
                                       ForwardedHeaders.XForwardedProto;

            if (!env.IsDevelopment())
            {
                options.KnownProxies.Clear();
                options.KnownNetworks.Clear();

                List<string>? trustedProxies =
                    configuration.GetSection("ForwardedHeaders:TrustedProxies").Get<List<string>>();
                if (trustedProxies != null)
                {
                    foreach (string proxy in trustedProxies)
                    {
                        options.KnownProxies.Add(IPAddress.Parse(proxy));
                    }
                }

                List<NetworkConfig>? trustedNetworks = configuration.GetSection("ForwardedHeaders:TrustedNetworks")
                    .Get<List<NetworkConfig>>();
                if (trustedNetworks != null)
                {
                    foreach (NetworkConfig network in trustedNetworks)
                    {
                        options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse(network.Prefix), network.PrefixLength));
                    }
                }
            }
        });

        return services;
    }
}

public class NetworkConfig
{
    public string Prefix { get; set; } = string.Empty;
    public int PrefixLength { get; set; }
}