using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserResult : IServiceResult
    {
        public UpdateUserResult(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public IUser User { get; }
    }

    public class UpdateUserArg : IServiceArgs<UpdateUserResult>, IUpdateUserArgs
    {
        public UpdateUserArg(string id, string firstName, string lastName, string email, string billingId,
            string authId, int versionNumber)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BillingId = billingId;
            AuthId = authId;
            VersionNumber = versionNumber;
        }

        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string BillingId { get; }
        public string AuthId { get; }
        public int VersionNumber { get; }
    }
}