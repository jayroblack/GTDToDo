using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Fakes
{
    public class FakeSendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        private readonly IMailTrap _mailTrap;

        public FakeSendEmailCommandHandler(IMailTrap mailTrap)
        {
            _mailTrap = mailTrap ?? throw new ArgumentNullException(nameof(mailTrap));
        }

        public async Task Run(SendEmailCommand command)
        {
            _mailTrap.Add(command);
        }
    }
}