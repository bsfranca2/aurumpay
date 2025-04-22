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

IResourceBuilder<ProjectResource> checkoutApi = builder
    .AddProject<Checkout_Api>("CheckoutApi")
    .WithReference(mainDbDatabase)
    .WaitFor(mainDbDatabase);

IResourceBuilder<NodeAppResource> checkoutApp = builder
    .AddNpmApp("CheckoutApp", "../../../apps/checkout", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithReference(checkoutApi)
    .WithEnvironment("NUXT_HOST", "0.0.0.0")
    .WithEnvironment("NODE_EXTRA_CA_CERTS", certConfig.RootCaPemPath)
    .WithOtlpExporter()
    .WaitFor(checkoutApi);

builder.AddContainer("caddy", "caddy", "2.7.4-alpine")
    .WithBindMount(certConfig.CertsSourcePath, "/etc/caddy/certs", true)
    .WithBindMount(certConfig.CaddyConfigPath, "/etc/caddy/Caddyfile", true)
    .WithHttpsEndpoint(8443, 443)
    .WithEnvironment("CHECKOUTAPP_URL", checkoutApp.GetEndpoint("http"))
    .WithEnvironment("CHECKOUTAPI_URL", checkoutApi.GetEndpoint("http"))
    .WithOtlpExporter()
    .WaitFor(checkoutApp)
    .WaitFor(checkoutApi);

builder.Build().Run();