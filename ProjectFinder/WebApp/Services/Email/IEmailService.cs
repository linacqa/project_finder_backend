namespace WebApp.Services.Email;

public interface IEmailService
{
    Task SendBulkAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default);
}

