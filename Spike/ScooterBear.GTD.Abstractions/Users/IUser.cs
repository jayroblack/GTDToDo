using System;

namespace ScooterBear.GTD.Abstractions.Users
{
    public interface IUser
    {
        string ID { get; }
        string Data { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        bool IsEmailVerified { get; }
        string BillingId { get; }
        string AuthId { get; }
        bool IsAccountEnabled { get; }
        DateTime DateCreated { get; }
        int? VersionNumber { get; }
    }
}
