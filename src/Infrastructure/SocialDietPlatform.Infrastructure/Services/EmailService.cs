using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SocialDietPlatform.Application.Interfaces.Services;
using SocialDietPlatform.Infrastructure.Configuration;

namespace SocialDietPlatform.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ISendGridClient _sendGridClient;

    public EmailService(IOptions<EmailSettings> emailSettings, ISendGridClient sendGridClient)
    {
        _emailSettings = emailSettings.Value;
        _sendGridClient = sendGridClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var from = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
        var toEmail = new EmailAddress(to);

        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, body);

        await _sendGridClient.SendEmailAsync(msg, cancellationToken);
    }

    public async Task SendWelcomeEmailAsync(string to, string firstName, CancellationToken cancellationToken = default)
    {
        var subject = "Sosyal Diyet Platformuna Hoş Geldiniz!";
        var body = $@"
            <h2>Merhaba {firstName}!</h2>
            <p>Sosyal Diyet Platformumuza hoş geldiniz. Sağlıklı yaşam yolculuğunuzda sizinle birlikte olmaktan mutluluk duyuyoruz.</p>
            <p>Platformumuzda neler yapabilirsiniz:</p>
            <ul>
                <li>Diyet planlarınızı takip edin</li>
                <li>Sağlıklı tarifler paylaşın</li>
                <li>Diğer kullanıcılarla etkileşime geçin</li>
                <li>Diyetisyenlerden destek alın</li>
            </ul>
            <p>İyi günler dileriz!</p>
        ";

        await SendEmailAsync(to, subject, body, cancellationToken);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetToken, CancellationToken cancellationToken = default)
    {
        var subject = "Şifre Sıfırlama";
        var resetUrl = $"{_emailSettings.WebsiteUrl}/reset-password?token={resetToken}";
        var body = $@"
            <h2>Şifre Sıfırlama Talebi</h2>
            <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
            <a href='{resetUrl}'>Şifremi Sıfırla</a>
            <p>Bu bağlantı 24 saat geçerlidir.</p>
        ";

        await SendEmailAsync(to, subject, body, cancellationToken);
    }

    public async Task SendAppointmentReminderAsync(string to, DateTime appointmentTime, CancellationToken cancellationToken = default)
    {
        var subject = "Randevu Hatırlatması";
        var body = $@"
            <h2>Randevu Hatırlatması</h2>
            <p>Yarın saat {appointmentTime:HH:mm}'da randevunuz bulunmaktadır.</p>
            <p>Randevu tarihi: {appointmentTime:dd.MM.yyyy}</p>
        ";

        await SendEmailAsync(to, subject, body, cancellationToken);
    }
}