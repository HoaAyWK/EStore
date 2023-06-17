namespace EStore.Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string mailTo, string body);
}
