using EStore.Application.Common.Interfaces.Services;
using EStore.Infrastructure.Services.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EStore.Infrastructure.Services;

internal sealed class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> mailOptions)
    {
        _mailSettings = mailOptions.Value;
    }

    public async Task SendEmailAsync(string subject, string mailTo, string body)
    {
        var email = new MimeMessage
        {
            From = { new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail ) },
            To = { MailboxAddress.Parse(mailTo) },
            Subject = subject,
            Body = new TextPart(TextFormat.Text) { Text = body }
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            _mailSettings.SmtpServer,
            _mailSettings.SmtpPort,
            SecureSocketOptions.StartTls);
        
        await smtpClient.AuthenticateAsync(
            _mailSettings.SenderEmail,
            _mailSettings.SmtpPassword);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}
