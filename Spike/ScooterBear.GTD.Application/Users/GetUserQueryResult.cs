using System;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
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
