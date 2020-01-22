using System;
using Optional;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class ExistingUser
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

        internal ExistingUser(string id, string firstName, string lastName, string email, bool isEmailVerified,
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

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"{nameof(firstName)} is required.");
            this.FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"{nameof(lastName)} is required.");
            this.LastName = lastName;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"{nameof(email)} is required.");

            if (email == this.Email)
                return;

            this.Email = email;
            this.IsEmailVerified = false;
            DisableAccount();
        }

        /// <summary>
        /// To Be set whenever a user clicks on a link we have sent to their email address provided.
        /// </summary>
        public void VerifyEmail()
        {
            this.IsEmailVerified = true;
            EnableAccount();
        }

        /// <summary>
        /// A user will pay for this service, this ID is our way to access this users payment history and status.
        /// </summary>
        public void SetBillingId(string billingId)
        {
            if( string.IsNullOrEmpty(billingId))
                throw new ArgumentException($"{nameof(billingId)} is required.");
            this.BillingId = billingId;
            EnableAccount();
        }

        /// <summary>
        /// Whenever a user is created, we use an external system for authentication, this string is what we need to load the current user.
        /// </summary>
        /// <param name="authId"></param>
        public void SetAuthId(string authId)
        {
            if (string.IsNullOrEmpty(authId))
                throw new ArgumentException($"{nameof(authId)} is required.");
            this.AuthId = authId;
            EnableAccount();
        }

        public enum EnableUserOutcome
        {
            EmailIsNotVerified,
            BillingIdIsNotDefined,
            AuthIdIsNotDefined,
        }

        public void DisableAccount()
        {
            this.IsAccountEnabled = false;
        }

        /// <summary>
        /// An account is Enabled once we have verified an email, have their billing information on file, and their authentication is set up.
        /// </summary>
        /// <returns>EnableUserOutcome</returns>
        public Option<bool, EnableUserOutcome> EnableAccount()
        {
            if( !this.IsEmailVerified)
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.EmailIsNotVerified);

            if( ! string.IsNullOrEmpty(this.BillingId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.BillingIdIsNotDefined);

            if (!string.IsNullOrEmpty(this.AuthId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.AuthIdIsNotDefined);

            this.IsAccountEnabled = true;
            return Option.Some<bool, EnableUserOutcome>(true);
        }
    }
}
