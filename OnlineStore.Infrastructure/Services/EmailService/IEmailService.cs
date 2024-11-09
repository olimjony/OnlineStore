using Domain.DTOs.EmailDTOs;
using MimeKit.Text;

namespace OnlineStore.Infrastructure.Services.EmailService;

public interface IEmailService
{
    public Task SendEmail(EmailMessageDto model, TextFormat format);
}