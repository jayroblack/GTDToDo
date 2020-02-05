using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserServiceResult : IServiceResult
    {
        public IUser User { get; }

        public UpdateUserServiceResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class UpdateUserServiceArgs : IServiceArgs<UpdateUserServiceResult>, IUpdateUserArgs
    {
        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public bool? IsEmailVerified { get; }
        public string BillingId { get; }
        public string AuthId { get; }
        public int VersionNumber { get; }

        public UpdateUserServiceArgs(string id, string firstName, string lastName, string email, bool? isEmailVerified, string billingId, string authId, int versionNumber)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsEmailVerified = isEmailVerified;
            BillingId = billingId;
            AuthId = authId;
            VersionNumber = versionNumber;
        }
    }
}
