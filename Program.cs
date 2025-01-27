using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetEnv;

namespace YourNamespace
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "twosum")
            {
                var twoSumDemo = new TwoSumDemo();
                twoSumDemo.RunTwoSumDemo();
                return;
            }

            if (args.Length > 0 && args[0] == "email")
            {
                Env.Load();

                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Create email config from environment variables
                        var emailConfig = new EmailConfig
                        {
                            SmtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER"),
                            Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "587"),
                            Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME"),
                            Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
                        };
                        
                        services.AddSingleton(emailConfig);
                        services.AddSingleton<EmailService>();
                        services.AddHostedService<EmailBackgroundService>();
                    })
                    .Build();

                await host.RunAsync();
            }

            if (args.Length > 0 && args[0] == "fakemail")
            {       
                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<DevelopmentEmailService>();
                        services.AddHostedService<FakeEmailBackgroundService>();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                    })
                    .Build();

                await host.RunAsync();
            }
        }
    }
}