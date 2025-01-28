using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class FakeEmailConfig
    {
        public string Username { get; set; }
        public string RecipientEmail { get; set; }
    }

    public class FakeEmailService
    {
        private readonly ILogger<FakeEmailService> _logger;
        private readonly FakeEmailConfig _config;

        public FakeEmailService(ILogger<FakeEmailService> logger, FakeEmailConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public Task SendEmailAsync(string to, string subject, string message)
        {
            // Log the email instead of actually sending it
            _logger.LogInformation(
                "Fake Email:\n" +
                $"From: {_config.Username}\n" +
                $"To: {to}\n" +
                $"Subject: {subject}\n" +
                $"Message: {message}\n" +
                $"Timestamp: {DateTime.Now}"
            );

            return Task.CompletedTask;
        }
    }

    public class FakeEmailBackgroundService : BackgroundService
    {
        private readonly FakeEmailService _emailService;
        private readonly ILogger<FakeEmailBackgroundService> _logger;
        private readonly FakeEmailConfig _config;

        public FakeEmailBackgroundService(FakeEmailService emailService, 
            ILogger<FakeEmailBackgroundService> logger,
            FakeEmailConfig config)
        {
            _emailService = emailService;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Send a test email every minute using config values
                    await _emailService.SendEmailAsync(
                        _config.RecipientEmail,
                        "Test Notification",
                        $"This is a test notification sent at {DateTime.Now}"
                    );

                    _logger.LogInformation("Test email logged successfully");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in background service: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }
    }
}