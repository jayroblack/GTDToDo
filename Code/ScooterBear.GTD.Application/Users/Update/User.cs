using System;
using Optional;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class User : IUser
    {
        public enum EnableUserOutcome
        {
            EmailIsNotVerified,
            BillingIdIsNotDefined,
            AuthIdIsNotDefined
        }

        //I would rather this be internal so that external users are unable to create this class
        //they have to go through the UpdateUserService.
        //But in order to make it testable - I have to open it up.
        public User(string id, string firstName, string lastName, string email,
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
            if (dateCreated == default)
                throw new ArgumentException($"Date Created value {dateCreated} is not valid.");

            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BillingId = billingId;
            AuthId = authId;
            VersionNumber = versionNumber;
            DateCreated = dateCreated;
            EnableAccount();
        }

        public string ID { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string BillingId { get; private set; }
        public string AuthId { get; private set; }
        public bool? IsAccountEnabled { get; private set; }
        public int VersionNumber { get; private set; }
        public DateTime DateCreated { get; }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"{nameof(firstName)} is required.");
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"{nameof(lastName)} is required.");
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"{nameof(email)} is required.");

            if (email == Email)
                return;

            Email = email;
            IsAccountEnabled = false;
        }

        /// <summary>
        ///     A user will pay for this service, this ID is our way to access this users payment history and status.
        /// </summary>
        public void SetBillingId(string billingId)
        {
            if (string.IsNullOrEmpty(billingId))
                throw new ArgumentException($"{nameof(billingId)} is required.");
            if (BillingId == billingId)
                return;
            BillingId = billingId;
            EnableAccount();
        }

        /// <summary>
        ///     Whenever a user is created, we use an external system for authentication, this string is what we need to load the
        ///     current user.
        /// </summary>
        /// <param name="authId"></param>
        public void SetAuthId(string authId)
        {
            if (string.IsNullOrEmpty(authId))
                throw new ArgumentException($"{nameof(authId)} is required.");
            if (AuthId == authId)
                return;
            AuthId = authId;
            EnableAccount();
        }

        public void SetVersionNumber(int versionNumber)
        {
            if (versionNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(versionNumber));
            VersionNumber = versionNumber;
        }

        /// <summary>
        ///     An account is Enabled once we have verified an email, have their billing information on file, and their
        ///     authentication is set up.
        /// </summary>
        /// <returns>EnableUserOutcome</returns>
        public Option<bool, EnableUserOutcome> EnableAccount()
        {
            if (string.IsNullOrEmpty(BillingId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.BillingIdIsNotDefined);

            if (string.IsNullOrEmpty(AuthId))
                return Option.None<bool, EnableUserOutcome>(EnableUserOutcome.AuthIdIsNotDefined);

            IsAccountEnabled = true;
            return Option.Some<bool, EnableUserOutcome>(true);
        }
    }
}