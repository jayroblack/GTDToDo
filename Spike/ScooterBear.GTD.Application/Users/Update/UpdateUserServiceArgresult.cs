using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserServiceResult : IServiceResult
    {

    }

    public class UpdateUserServiceArgs : IServiceArgs<UpdateUserServiceResult>, IUser
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsEmailVerified{ get; set; }
        public string BillingId { get; set; }
        public string AuthId { get; set; }
        public bool? IsAccountEnabled { get; set; }
        public int VersionNumber { get; set; }
    }
}
