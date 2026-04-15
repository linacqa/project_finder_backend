namespace WebApp.Services.Email;

public class SmtpOptions
{
    public const string SectionName = "Email:Smtp";

    public string Host { get; set; } = default!;
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;

    public string? UserName { get; set; }
    public string? Password { get; set; }

    public string FromAddress { get; set; } = default!;
    public string? FromName { get; set; }
}

