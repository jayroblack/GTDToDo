using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application.Users.New
{
    public class WhenNewUserCreatedBootstrapUser : IDomainEventHandlerAsync<NewUserCreatedEvent>
    {
        public Task HandleAsync(NewUserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            //TODO:  Create Default Project "Inbox" -> Default Project all Tasks begin.
            throw new NotImplementedException();
        }
    }
}
