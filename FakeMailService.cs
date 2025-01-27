using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class DevelopmentEmailService
    {
        private readonly ILogger<DevelopmentEmailService> _logger;

        public DevelopmentEmailService(ILogger<DevelopmentEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string message)
        {
            // Log the email instead of actually sending it
            _logger.LogInformation(
                "Development Email:\n" +
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
        private readonly DevelopmentEmailService _emailService;
        private readonly ILogger<FakeEmailBackgroundService> _logger;

        public FakeEmailBackgroundService(DevelopmentEmailService emailService, ILogger<FakeEmailBackgroundService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Send a test email every minute
                    await _emailService.SendEmailAsync(
                        "test@example.com",
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