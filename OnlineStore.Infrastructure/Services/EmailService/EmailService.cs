using Domain.DTOs.EmailDTOs;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace OnlineStore.Infrastructure.Services.EmailService;

public class EmailService(EmailConfiguration configuration) : IEmailService
{
    public async Task SendEmail(EmailMessageDto message, TextFormat format)
    {
        var emailMessage = CreateEmailMessage(message, format);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailMessageDto message, TextFormat format)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("mail", configuration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(format) { Text = message.Content };

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        
        await client.ConnectAsync(configuration.SmtpServer, configuration.Port, true);
        client.AuthenticationMechanisms.Remove("OAUTH2");
        await client.AuthenticateAsync(configuration.UserName, configuration.Password);

        await client.SendAsync(mailMessage);
        await client.DisconnectAsync(true);
        
        client.Dispose();
    }
}