using System.Collections.Generic;
using ScooterBear.GTD.Application.Services.Email;

namespace ScooterBear.GTD.Fakes
{
    public interface IMailTrap
    {
        void Add(SendEmailCommand command);
        List<SendEmailCommand> ForEmail(string toEmail);
    }

    public class MailTrap : IMailTrap
    {
        private Dictionary<string, List<SendEmailCommand>> _sentEmails;

        public MailTrap()
        {
            _sentEmails = new Dictionary<string, List<SendEmailCommand>>();
        }

        public void Add(SendEmailCommand command)
        {
            if (!_sentEmails.ContainsKey(command.ToEmail))
                _sentEmails.Add(command.ToEmail, new List<SendEmailCommand>());

            _sentEmails[command.ToEmail].Add(command);
        }

        public List<SendEmailCommand> ForEmail(string toEmail)
        {
            return _sentEmails[toEmail];
        }
    }
}
