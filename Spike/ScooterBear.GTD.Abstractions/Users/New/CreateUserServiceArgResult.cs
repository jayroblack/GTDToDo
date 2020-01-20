using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users.New
{
    public class CreateUserServiceResult : IServiceResult{
        public ReadonlyUser User { get; }

        public CreateUserServiceResult(ReadonlyUser user)
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

        public CreateUserServiceArg(string id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
