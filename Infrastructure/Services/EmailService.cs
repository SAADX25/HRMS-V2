using System.Net;
using System.Net.Mail;
using Application.DTOs.Email;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(EmailDto emailDto)
    {
        try
        {
            var smtpServer = configuration["EmailSettings:SmtpServer"];
            var smtpPortString = configuration["EmailSettings:SmtpPort"];
            var senderEmail = configuration["EmailSettings:SenderEmail"];
            var senderPassword = configuration["EmailSettings:SenderPassword"];

            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPortString) ||
                string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
            {
                logger.LogWarning("EmailSettings are not fully configured. Email was not sent.");
                return;
            }

            if (!int.TryParse(smtpPortString, out int smtpPort))
            {
                smtpPort = 587; // default
            }

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HRMS System"),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.ToEmail);

            await client.SendMailAsync(mailMessage);
            logger.LogInformation("Email sent successfully to {ToEmail}", emailDto.ToEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {ToEmail}", emailDto.ToEmail);
            throw; // Re-throw to be handled by the global exception handler
        }
    }
}
