using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Email
{
    public class SendEmailCommand : ICommand
    {
        public string FromEmail { get; }
        public string ToEmail { get; }
        public string Subject { get; }
        public string TextBody { get; }
        public string HtmlBody { get; }

        public SendEmailCommand(string fromEmail, string toEmail, string subject, string textBody, string htmlBody)
        {
            if (string.IsNullOrEmpty(fromEmail))
                throw new ArgumentException($"{nameof(fromEmail)} is required.");
            if (string.IsNullOrEmpty(toEmail))
                throw new ArgumentException($"{nameof(toEmail)} is required.");
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentException($"{nameof(subject)} is required.");
            if (string.IsNullOrEmpty(textBody))
                throw new ArgumentException($"{nameof(textBody)} is required.");
            if (string.IsNullOrEmpty(htmlBody))
                throw new ArgumentException($"{nameof(htmlBody)} is required.");

            FromEmail = fromEmail;
            ToEmail = toEmail;
            Subject = subject;
            TextBody = textBody;
            HtmlBody = htmlBody;
        }
    }
}
