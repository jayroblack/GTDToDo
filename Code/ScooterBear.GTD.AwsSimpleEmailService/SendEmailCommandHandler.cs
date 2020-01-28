using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AwsSimpleEmailService
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        public Task Run(SendEmailCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
