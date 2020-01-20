using System;

namespace ScooterBear.GTD.Abstractions.Users.New
{
    public interface INewuser
    {
        string ID { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        DateTime DateCreated { get; }
    }
}
