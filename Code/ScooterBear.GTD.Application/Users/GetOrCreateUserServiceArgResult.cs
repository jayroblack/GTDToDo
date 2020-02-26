using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
{
    public class GetOrCreateUserServiceResult : IServiceResult
    {
        public IUser User { get; }

        public GetOrCreateUserServiceResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class GetOrCreateUserServiceArgs : IServiceArgs<GetOrCreateUserServiceResult>
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public GetOrCreateUserServiceArgs(string id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
