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

    public class UpdateUserServiceArgs : IServiceArgs<UpdateUserServiceResult>, IUser
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string BillingId { get; set; }
        public string AuthId { get; set; }
        public bool? IsAccountEnabled { get; set; }
        public int VersionNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
