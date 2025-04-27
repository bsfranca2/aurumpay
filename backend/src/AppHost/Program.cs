using AurumPay.AppHost;

using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

CertificateConfig certConfig = CertsHelper.SetupCertificates(builder.AppHostDirectory);

IResourceBuilder<ParameterResource> mainDbUserName = builder.AddParameter("postgres-username");
IResourceBuilder<ParameterResource> mainDbPassword = builder.AddParameter("postgres-password");
IResourceBuilder<PostgresServerResource> mainDb = builder
    .AddPostgres("postgres", mainDbUserName, mainDbPassword, 5432)
    .WithDataVolume();
IResourceBuilder<PostgresDatabaseResource> mainDbDatabase = mainDb.AddDatabase("Database", "AurumPayDev");

IResourceBuilder<ValkeyResource> cache = builder
    .AddValkey("Cache")
    .WithDataVolume();

IResourceBuilder<ProjectResource> checkoutApi = builder
    .AddProject<Checkout_Api>("CheckoutApi")
    .WithReference(mainDbDatabase)
    .WaitFor(mainDbDatabase)
    .WithReference(cache);

IResourceBuilder<NodeAppResource> checkoutApp = builder
    .AddNpmApp("CheckoutApp", "../../../apps/checkout", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithReference(checkoutApi)
    .WithEnvironment("NUXT_HOST", "0.0.0.0")
    .WithEnvironment("NODE_EXTRA_CA_CERTS", certConfig.RootCaPemPath)
    .WithEnvironment("NUXT_API_URL", checkoutApi.GetEndpoint("http"))
    .WaitFor(checkoutApi);

builder.AddContainer("CheckoutGateway", "caddy", "2.7.4-alpine")
    .WithBindMount(certConfig.CertsSourcePath, "/etc/caddy/certs", true)
    .WithBindMount(certConfig.CaddyConfigPath, "/etc/caddy/Caddyfile", true)
    .WithHttpsEndpoint(8443, 443)
    .WithEnvironment("CHECKOUTAPP_URL", checkoutApp.GetEndpoint("http"))
    .WaitFor(checkoutApp);

builder.Build().Run();