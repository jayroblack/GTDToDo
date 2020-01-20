using System;
using ScooterBear.GTD.Abstractions.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class GetUserQueryResult : IQueryResult
    {
        public INewuser User { get; }

        public GetUserQueryResult(INewuser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
