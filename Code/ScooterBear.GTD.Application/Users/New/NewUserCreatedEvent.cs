using System;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application.Users.New
{
    public class NewUserCreatedEvent : IDomainEvent
    {
        public IUser User { get; }

        public NewUserCreatedEvent(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
