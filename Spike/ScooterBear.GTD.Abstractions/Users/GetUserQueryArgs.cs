using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class GetUserQueryArgs : IQuery<GetUserQueryResult>
    {
        public string UserId { get; }

        public GetUserQueryArgs(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"'{userId}' is not a valid User Id.");
            UserId = userId;
        }
    }
}
