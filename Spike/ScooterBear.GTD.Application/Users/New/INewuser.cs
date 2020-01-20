using System;

namespace ScooterBear.GTD.Application.Users.New
{
    public interface INewUser
    {
        string ID { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        DateTime DateCreated { get; }
    }
}
