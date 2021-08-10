using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace CertTestPoc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var site1 = CreateHostBuilder(args).Build();
            var site2 = CreateHostBuilder2(args).Build();

            await Task.WhenAny(
                site1.RunAsync(),
                site2.RunAsync()
            ).Result;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                    webBuilder.ConfigureKestrel(o =>
                    {
                        o.ConfigureHttpsDefaults(x =>
                        {
                            x.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                            x.AllowAnyClientCertificate();
                        });
                    });
                });

        public static IHostBuilder CreateHostBuilder2(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5002", "https://*:5003");
                    webBuilder.UseStartup<StartUp2>();
                });
    }
}
