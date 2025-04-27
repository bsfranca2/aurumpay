using System.Diagnostics;

namespace AurumPay.AppHost;

internal static class CertsHelper
{
    public static CertificateConfig SetupCertificates(string appHostDirectory)
    {
        string mkcertCaRootPath = GetMkcertCaRootPath();

        string rootCaPemPath = Path.Combine(mkcertCaRootPath, "rootCA.pem");
        if (!File.Exists(rootCaPemPath))
        {
            throw new FileNotFoundException(
                $"Error: mkcert root CA file 'rootCA.pem' not found in the CAROOT directory: '{mkcertCaRootPath}'. Please ensure mkcert setup is complete (`mkcert -install`).");
        }

        string certsSourcePath = Path.Combine(appHostDirectory, "certs");
        string caddyConfigSourcePath = Path.Combine(appHostDirectory, "config/Caddyfile");
        string certFile = Path.Combine(certsSourcePath, "local-dev-cert.pem");
        string keyFile = Path.Combine(certsSourcePath, "local-dev-key.pem");

        VerifyFileExistence(certsSourcePath, true);
        VerifyFileExistence(certFile);
        VerifyFileExistence(keyFile);
        VerifyFileExistence(caddyConfigSourcePath);

        return new CertificateConfig(certsSourcePath, caddyConfigSourcePath, certFile, keyFile, rootCaPemPath);
    }

    private static string GetMkcertCaRootPath()
    {
        try
        {
            using Process process = new();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "mkcert",
                Arguments = "-CAROOT",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            string caRootPath = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(caRootPath))
            {
                throw new InvalidOperationException(
                    $"'mkcert -CAROOT' failed or returned empty path. Exit Code: {process.ExitCode}");
            }

            Console.WriteLine($"INFO: Found mkcert CAROOT path: {caRootPath}");
            return caRootPath;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Error finding mkcert CA root path. Make sure 'mkcert' is installed and accessible in the system PATH.",
                ex);
        }
    }

    private static void VerifyFileExistence(string path, bool isDirectory = false)
    {
        if (isDirectory)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Error: Directory not found at '{path}'. Please create it.");
            }
        }
        else if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Error: File not found at '{path}'. Please ensure it exists.");
        }
    }
}

internal record CertificateConfig(
    string CertsSourcePath,
    string CaddyConfigPath,
    string CertFile,
    string KeyFile,
    string RootCaPemPath);