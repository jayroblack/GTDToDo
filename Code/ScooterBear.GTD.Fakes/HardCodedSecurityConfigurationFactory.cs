using ScooterBear.GTD.Application.Services.Security;

namespace ScooterBear.GTD.Fakes
{
    public class HardCodedSecurityConfigurationFactory : ISecurityConfigurationFactory
    {
        public SecurityConfiguration Get()
        {
            return new SecurityConfiguration("7A24432646294A404E635266556A586E327235753878214125442A472D4B6150");
        }
    }
}