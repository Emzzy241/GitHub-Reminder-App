using MailKit.Net.Smtp;
using MimeKit;

namespace GitHubReminderApp.Services;

public class EmailService
{
    private readonly string _fromEmail;
    private readonly string _password;
    private readonly string _smtpServer;
    private readonly int _smtpPort;

    public EmailService()
    {
        _fromEmail = Environment.GetEnvironmentVariable("FROM_EMAIL");
        _password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
        _smtpPort  = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("GitHub Reminder", _fromEmail));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = body };

        using (var smtp = new SmtpClient())
        {
            smtp.Connect(_smtpServer, _smtpPort, false);
            smtp.Authenticate(_fromEmail, _password);
            smtp.Send(email);
            smtp.Disconnect(true);
            
            // smtp.Connect("smtp.gmail.com", 587, false);
            // smtp.Authenticate(_fromEmail, _password);
            // smtp.Send(email);
            // smtp.Disconnect(true);
        }
    }
}
