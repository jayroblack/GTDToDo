using ScooterBear.GTD.Application.Services.Email;

namespace ScooterBear.GTD.Fakes
{
    public class HardCodedEmailConfigurationFactory : IEmailConfigurationFactory
    {
        public EmailConfiguration Create()
        {
            return new EmailConfiguration("SpruceGoose@here.com", null);
        }
    }
}