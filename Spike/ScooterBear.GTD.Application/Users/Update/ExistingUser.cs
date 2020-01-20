using Optional;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class ExistingUser : IUser
    {
        public string ID { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public string BillingId { get; private set; }
        public string AuthId { get; private set; }
        public bool IsAccountEnabled { get; private set; }
        public int VersionNumber { get; }

        public ExistingUser(string id, string firstName, string lastName, string email, bool isEmailVerified,
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

        public enum ModifyUserOutcome
        {
            EmailIsNotVerified,
            BillingIdIsNotDefined,
            AuthIdIsNotDefined,
            PaymentOverdue,
            DoesNotExist,
            Conflict
        }

        public void VerifyEmail()
        {
            this.IsEmailVerified = true;
        }

        public void SetBillingId(string billingId)
        {
            this.BillingId = billingId;
        }

        public void SetAuthId(string authId)
        {
            this.AuthId = authId;
        }

        public Option<bool, ModifyUserOutcome> EnableAccount()
        {
            if( !this.IsEmailVerified)
                return Option.None<bool, ModifyUserOutcome>(ModifyUserOutcome.EmailIsNotVerified);

            if( ! string.IsNullOrEmpty(this.BillingId))
                return Option.None<bool, ModifyUserOutcome>(ModifyUserOutcome.BillingIdIsNotDefined);

            if (!string.IsNullOrEmpty(this.AuthId))
                return Option.None<bool, ModifyUserOutcome>(ModifyUserOutcome.AuthIdIsNotDefined);

            this.IsAccountEnabled = true;
            return Option.Some<bool, ModifyUserOutcome>(true);
        }
    }
}
