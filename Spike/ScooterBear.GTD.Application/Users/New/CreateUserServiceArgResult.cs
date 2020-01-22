using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public class CreateUserServiceResult : IServiceResult
    {
        public IUser User { get; }

        public CreateUserServiceResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class CreateUserServiceArg : IServiceArgs<CreateUserServiceResult>
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string AuthId { get; }

        public CreateUserServiceArg(string id, string firstName, string lastName, string email, string authId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AuthId = authId;
        }
    }
}
