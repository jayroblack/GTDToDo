using System;
using Optional;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class User : IUser
    {
        public string ID { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public bool? IsEmailVerified { get; private set; }
        public string BillingId { get; private set; }
        public string AuthId { get; private set; }
        public bool? IsAccountEnabled { get; private set; }
        public int VersionNumber { get; private set; }
        public DateTime DateCreated { get; }

        //I would rather this be internal so that external users are unable to create this class
        //they have to go through the UpdateUserService.
        //But in order to make it testable - I have to open it up.
        public User(string id, string firstName, string lastName, string email, bool? isEmailVerified,
            string billingId, string authId, int versionNumber, DateTime dateCreated)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"Missing required value {nameof(id)}");
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"Missing required value {nameof(firstName)}");
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"Missing required value {nameof(lastName)}");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"Missing required value {nameof(email)}");
            if (versionNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(versionNumber));
            if( dateCreated == default(DateTime))
                throw new ArgumentException($"Date Created value {dateCreated} is not valid.");

            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsEmailVerified = isEmailVerified;
            BillingId = billingId;
            AuthId = authId;
            VersionNumber = versionNumber;
            DateCreated = dateCreated;
            EnableAccount();
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
            this.IsAccountEnabled = false;
        }

        /// <summary>
        /// To Be set whenever a user clicks on a link we have sent to their email address provided.
        /// </summary>
        public void VerifyEmail()
        {
            if (this.IsEmailVerified.GetValueOrDefault())
                return;

            this.IsEmailVerified = true;
            EnableAccount();
        }

        /// <summary>
        /// A user will pay for this service, this ID is our way to access this users payment history and status.
        /// </summary>
        public void SetBillingId(string billingId)
        {
            if (string.IsNullOrEmpty(billingId))
                throw new ArgumentException($"{nameof(billingId)} is required.");
            if (this.BillingId == billingId)
                return;
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
            if (this.AuthId == authId)
                return;
            this.AuthId = authId;
            EnableAccount();
        }

        public void SetVersionNumber(int versionNumber)
        {
            if( versionNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(versionNumber));
            this.VersionNumber = versionNumber;
        }

        public enum EnableUserOutcome
        {
            EmailIsNotVerified,
            BillingIdIsNotDefined,
            AuthIdIsNotDefined,
        }

        /// <summary>
        /// An account is Enabled once we have verified an email, have their billing information on file, and their authentication is set up.
        /// </summary>
        /// <returns>EnableUserOutcome</returns>
        public Option<bool, EnableUserOutcome> EnableAccount()
        {
            if (!this.IsEmailVerified.GetValueOrDefault())
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.EmailIsNotVerified);

            if (string.IsNullOrEmpty(this.BillingId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.BillingIdIsNotDefined);

            if (string.IsNullOrEmpty(this.AuthId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.AuthIdIsNotDefined);

            this.IsAccountEnabled = true;
            return Option.Some<bool, EnableUserOutcome>(true);
        }
    }
}