using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

#region Certs

string? mkcertCaRootPath = null;
try
{
    // Tenta executar 'mkcert -CAROOT' para obter o caminho
    using var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "mkcert", // Assume mkcert está no PATH
            Arguments = "-CAROOT",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        }
    };
    process.Start();
    mkcertCaRootPath = process.StandardOutput.ReadToEnd().Trim();
    process.WaitForExit();

    if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(mkcertCaRootPath))
    {
        throw new InvalidOperationException($"'mkcert -CAROOT' failed or returned empty path. Exit Code: {process.ExitCode}");
    }
    Console.WriteLine($"INFO: Found mkcert CAROOT path: {mkcertCaRootPath}");
}
catch (Exception ex)
{
    // Se falhar (mkcert não instalado/não no path), lança um erro claro
    throw new InvalidOperationException("Error finding mkcert CA root path. Make sure 'mkcert' is installed and accessible in the system PATH.", ex);
}

var rootCaPemPath = Path.Combine(mkcertCaRootPath, "rootCA.pem");
if (!File.Exists(rootCaPemPath))
{
    throw new FileNotFoundException($"Error: mkcert root CA file 'rootCA.pem' not found in the CAROOT directory: '{mkcertCaRootPath}'. Please ensure mkcert setup is complete (`mkcert -install`).");
}

var appHostDirectory = builder.AppHostDirectory;
var certsSourcePath = Path.Combine(appHostDirectory, "certs");
var caddyConfigSourcePath = Path.Combine(appHostDirectory, "config/Caddyfile");
var certFile = Path.Combine(certsSourcePath, "local-dev-cert.pem");
var keyFile = Path.Combine(certsSourcePath, "local-dev-key.pem");
if (!Directory.Exists(certsSourcePath))
{
    throw new DirectoryNotFoundException($"Error: Certificate source directory not found at '{certsSourcePath}'. Please create it.");
}
if (!File.Exists(certFile))
{
    throw new FileNotFoundException($"Error: Certificate file not found at '{certFile}'. Please generate it using mkcert.");
}
if (!File.Exists(keyFile))
{
    throw new FileNotFoundException($"Error: Certificate key file not found at '{keyFile}'. Please generate it using mkcert.");
}
if (!File.Exists(caddyConfigSourcePath))
{
    throw new FileNotFoundException($"Error: Caddyfile not found at '{caddyConfigSourcePath}'. Please create it.");
}

#endregion

var mainDbUserName = builder.AddParameter("postgres-username");
var mainDbPassword = builder.AddParameter("postgres-password");

var mainDb = builder
    .AddPostgres("postgres", mainDbUserName, mainDbPassword, port: 5432)
    .WithDataVolume();

var mainDbDatabase = mainDb.AddDatabase("Database", "AurumPayDev");

var checkoutApi = builder
    .AddProject<Projects.Checkout_Api>("CheckoutApi")
    .WithReference(mainDbDatabase)
    .WaitFor(mainDbDatabase);

var checkoutApp = builder
    .AddNpmApp("CheckoutApp", "../../../apps/checkout", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithReference(checkoutApi)
    .WithEnvironment("NUXT_HOST", "0.0.0.0")
    .WithEnvironment("NODE_EXTRA_CA_CERTS", rootCaPemPath)
    .WithOtlpExporter()
    .WaitFor(checkoutApi);

builder.AddContainer("caddy", "caddy", "2.7.4-alpine")
    .WithBindMount(certsSourcePath, "/etc/caddy/certs", isReadOnly: true)
    .WithBindMount(caddyConfigSourcePath, "/etc/caddy/Caddyfile", isReadOnly: true)
    .WithHttpsEndpoint(port: 8443, targetPort: 443, name: "caddy-https")
    .WithEnvironment("CHECKOUTAPP_URL", checkoutApp.GetEndpoint("http"))
    .WithEnvironment("CHECKOUTAPI_URL", checkoutApi.GetEndpoint("http"))
    .WithOtlpExporter()
    .WaitFor(checkoutApp)
    .WaitFor(checkoutApi);

builder.Build().Run();