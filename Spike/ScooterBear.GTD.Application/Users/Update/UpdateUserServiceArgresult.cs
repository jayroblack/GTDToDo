using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserServiceResult : IServiceResult
    {

    }

    public class UpdateUserServiceArgs : IServiceArgs<UpdateUserServiceResult>
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? EmailVerifiedValue { get; set; }
        public string BillingIdValue { get; set; }
        public string AuthIdValue { get; set; }
        public bool? AccountEnabledValue { get; set; }
        public int VersionNumber { get; set; }
    }
}
