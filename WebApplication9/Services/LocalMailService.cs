using Microsoft.Extensions.Options;
using WebApplication9.Models;

namespace WebApplication9.Services
{
    public class LocalMailService : IMailService
    {
        MailSettings _mailSettings;

        public LocalMailService(IOptions<MailSettings> mailSettings)//Options pattern
        {
            _mailSettings = mailSettings.Value;
        }

        public void Send(string subject, string message)
        {
            //send mail - output to console window
            Console.WriteLine($"Mail from {_mailSettings.MailFromAddress} to {_mailSettings.MailToAddress}, " + $" with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
