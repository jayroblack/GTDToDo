using System;

namespace ScooterBear.GTD.Application.Users
{
    public interface IUpdateUserArgs
    {
        string ID { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string BillingId { get; }
        string AuthId { get; }
        int VersionNumber { get; }
    }

    public interface IUser
    {
        string ID { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string BillingId { get; }
        string AuthId { get; }
        bool? IsAccountEnabled { get; }
        int VersionNumber { get; }
        DateTime DateCreated { get; }
    }
}
