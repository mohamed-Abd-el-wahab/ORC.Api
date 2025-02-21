using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace ORC.Api.Models
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlContent, string userName);
    }


    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _templatePath;

        public EmailService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            if (string.IsNullOrEmpty(environment.WebRootPath))
            {
                _logger.LogError("WebRootPath is null or empty.");
                throw new ArgumentNullException(nameof(environment.WebRootPath), "WebRootPath cannot be null or empty.");
            }
            _templatePath = Path.Combine(environment.WebRootPath, "EmailTemplates", "EmailTemplate.html");
            logger.LogInformation($"Template path set to {_templatePath}");
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent, string userName)
        {
            try
            {
                _logger.LogInformation($"Starting email send process to {to}");
                string fromMail = _configuration["EmailSettings:SenderEmail"];
                string fromPassword = _configuration["EmailSettings:Password"];

                _logger.LogInformation($"Creating mail message from {fromMail}");
                using var message = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = subject,
                    Body = await GetEmailBodyAsync(htmlContent, userName), 
                    IsBodyHtml = true
                };
                message.To.Add(new MailAddress(to));

                _logger.LogInformation($"Configuring SMTP client for {_configuration["EmailSettings:SmtpServer"]}:{_configuration["EmailSettings:Port"]}");
                using var smtpClient = new SmtpClient()
                {
                    Host = _configuration["EmailSettings:SmtpServer"],
                    Port = int.Parse(_configuration["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 30000 // 30 seconds timeout
                };

                _logger.LogInformation("Attempting to send email...");
                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}. Error details: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        private async Task<string> GetEmailBodyAsync(string messageContent, string userName)
        {
            try
            {
                if (!File.Exists(_templatePath))
                {
                    _logger.LogError($"Template file not found at {_templatePath}");
                    return messageContent;  // Return the plain message if the template doesn't exist
                }

                string template = await File.ReadAllTextAsync(_templatePath);
                template = template.Replace("{{content}}", messageContent);
                template = template.Replace("{{name}}", userName);  // Replace the user's name
                return template;
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogError(uaEx, $"Access to email template file denied: {_templatePath}");
                throw new UnauthorizedAccessException($"Insufficient permissions to access email template file: {_templatePath}", uaEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading email template at {_templatePath}, using plain message content.");
                return messageContent;  // Fallback to plain content if reading the template fails
            }
        }
    }
}