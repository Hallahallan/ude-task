using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace EmailNotificationService
{
    // Email configuration class to store settings
    public class EmailConfig
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "kkekesen@gmail.com";
        public string Password { get; set; } = "KekerOgReker";
    }

    // Email service to handle sending emails
    public class EmailService
    {
        private readonly EmailConfig _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailConfig config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                using var client = new SmtpClient(_config.SmtpServer, _config.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_config.Username, _config.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config.Username),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }

    // Background service that handles the email notifications
    public class EmailBackgroundService : BackgroundService
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailBackgroundService> _logger;

        public EmailBackgroundService(EmailService emailService, ILogger<EmailBackgroundService> logger)
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
                    // Example: Send a test email every minute
                    await _emailService.SendEmailAsync(
                        "martin.hallan@gmail.com",
                        "Test Notification",
                        $"This is a test notification sent at {DateTime.Now}"
                    );

                    // Wait for 1 minute before sending the next email
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in background service: {ex.Message}");
                    // Wait for 30 seconds before retrying if there's an error
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }
    }
}