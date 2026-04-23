using Application.DTOs.Email;

namespace Application.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
}
