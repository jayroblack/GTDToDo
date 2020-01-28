using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Patterns.CQRS;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application.Users.New
{
    public class WhenNewUserCreatedSendValidateEmail : IDomainEventHandlerAsync<NewUserCreatedEvent>
    {
        private readonly IService<SendNewUserEmailServiceArgs, SendNewUserEmailServiceResult> _sendNewUserEmail;

        public WhenNewUserCreatedSendValidateEmail(
            IService<SendNewUserEmailServiceArgs, SendNewUserEmailServiceResult> sendNewUserEmail)
        {
            _sendNewUserEmail = sendNewUserEmail ?? throw new ArgumentNullException(nameof(sendNewUserEmail));
        }
        public async Task HandleAsync(NewUserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
           await _sendNewUserEmail.Run(new SendNewUserEmailServiceArgs(domainEvent.User));
        }
    }
}
