using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class CreateUserCommand : ICommand
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public CreateUserCommand(string id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
