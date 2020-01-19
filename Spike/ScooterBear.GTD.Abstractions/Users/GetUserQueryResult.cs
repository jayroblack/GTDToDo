using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class GetUserQueryResult : IQueryResult
    {
        public IUser User { get; }

        public GetUserQueryResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
