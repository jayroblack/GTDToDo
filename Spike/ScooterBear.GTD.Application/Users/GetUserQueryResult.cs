using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
{
    public class GetUserQueryResult : IQueryResult
    {
        public ReadonlyUser User { get; }


        public GetUserQueryResult(ReadonlyUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
