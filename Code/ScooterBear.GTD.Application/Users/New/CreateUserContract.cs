using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public class CreateUserResult : IServiceResult
    {
        public CreateUserResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public IUser User { get; }
    }

    public class CreateUserArg : IServiceArgs<CreateUserResult>
    {
        public CreateUserArg(string id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}