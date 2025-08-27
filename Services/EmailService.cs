using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using BornOtomasyonApi.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            // Ayarları al
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPortStr = _configuration["EmailSettings:SmtpPort"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            // Eksik ayar kontrolü 
            if (string.IsNullOrWhiteSpace(smtpServer) ||
                string.IsNullOrWhiteSpace(smtpPortStr) ||
                string.IsNullOrWhiteSpace(senderName) ||
                string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Email ayarları appsettings.json içinde eksik veya boş.");
            }

            if (!int.TryParse(smtpPortStr, out int smtpPort))
            {
                throw new Exception("EmailSettings:SmtpPort değeri geçerli bir sayı değil.");
            }

            // Mail mesajı oluşturur.
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            // SMTP ile gönderir
            using var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
            Console.WriteLine($" Email başarıyla gönderildi: {toEmail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Email gönderim hatası: {ex.Message}");
            throw; // bir üst atmana hatayı iletir.
        }
    }
}
