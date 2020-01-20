
namespace ScooterBear.GTD.Abstractions.Users
{
    public class ReadonlyUser : IUser
    {
        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public bool IsEmailVerified { get; }
        public string BillingId { get; }
        public string AuthId { get; }
        public bool IsAccountEnabled { get; }
        public int VersionNumber { get; }

        public ReadonlyUser(string id, string firstName, string lastName, string email, bool isEmailVerified,
            string billingId, string authId, bool isAccountEnabled, int versionNumber)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsEmailVerified = isEmailVerified;
            BillingId = billingId;
            AuthId = authId;
            IsAccountEnabled = isAccountEnabled;
            VersionNumber = versionNumber;
        }
    }
}
