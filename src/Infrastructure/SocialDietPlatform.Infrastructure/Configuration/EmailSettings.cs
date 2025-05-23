namespace SocialDietPlatform.Infrastructure.Configuration;

public class EmailSettings
{
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public string WebsiteUrl { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public bool EnableSsl { get; set; }

    public EmailSettings()
    {
        FromEmail = string.Empty;
        FromName = string.Empty;
        WebsiteUrl = string.Empty;
        SmtpServer = string.Empty;
        SmtpUsername = string.Empty;
        SmtpPassword = string.Empty;
    }
} 