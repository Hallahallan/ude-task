using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetEnv;

namespace YourNamespace
{
    public class Program
    {
        private static readonly TwoSumDemo _twoSumDemo = new TwoSumDemo();

        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Specify a command: twosum-basic, twosum-fibo, twosum-all, email, or fakemail");
                return;
            }

            switch (args[0].ToLower())
            {
                case "twosum-basic":
                    _twoSumDemo.RunBasicTest();
                    break;

                case "twosum-fibo":
                    _twoSumDemo.RunFibonacciTest();
                    break;

                case "twosum-all":
                    _twoSumDemo.RunAllTests();
                    break;

                case "email":
                    await RunEmailService(args);
                    break;

                case "fakemail":
                    await RunFakeEmailService(args);
                    break;

                default:
                    Console.WriteLine("Invalid command. Available commands: twosum-basic, twosum-fibo, twosum-all, email, or fakemail");
                    break;
            }
        }

        private static async Task RunEmailService(string[] args)
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

        private static async Task RunFakeEmailService(string[] args)
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