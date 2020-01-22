namespace ScooterBear.GTD.Application.Users
{
    public interface IUser
    {
        string ID { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        bool IsEmailVerified { get; }
        string BillingId { get; }
        string AuthId { get; }
        bool IsAccountEnabled { get; }
        int VersionNumber { get; }
    }
}
