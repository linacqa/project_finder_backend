using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace WebApp.Services.Email;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpOptions _options;

    public SmtpEmailService(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendBulkAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Host) || string.IsNullOrWhiteSpace(_options.FromAddress))
        {
            throw new InvalidOperationException("SMTP settings are incomplete. Configure Email:Smtp:Host and Email:Smtp:FromAddress.");
        }

        var recipientList = recipients
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Select(r => r.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (recipientList.Count == 0)
        {
            return;
        }

        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        if (!string.IsNullOrWhiteSpace(_options.UserName))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_options.UserName, _options.Password);
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        foreach (var recipient in recipientList)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var message = new MailMessage
            {
                From = string.IsNullOrWhiteSpace(_options.FromName)
                    ? new MailAddress(_options.FromAddress)
                    : new MailAddress(_options.FromAddress, _options.FromName),
                // Bcc = {new MailAddress(_options.FromAddress)},
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(recipient);
            await client.SendMailAsync(message, cancellationToken);
        }
    }
}

