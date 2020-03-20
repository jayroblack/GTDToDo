using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
{
    public class GetUserArg : IQuery<GetUserQueryResult>
    {
        public GetUserArg(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"'{userId}' is not a valid User Id.");
            UserId = userId;
        }

        public string UserId { get; }
    }

    public class GetUserQueryResult : IQueryResult
    {
        public GetUserQueryResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public IUser User { get; }
    }
}