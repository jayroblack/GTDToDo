using System;

namespace ScooterBear.GTD.Application.Services.Security
{
    public interface ISecurityConfigurationFactory
    {
        SecurityConfiguration Get();
    }

    public class SecurityConfiguration
    {
        public string EmailEncryption { get; }

        public SecurityConfiguration(string emailEncryption)
        {
            EmailEncryption = emailEncryption ?? throw new ArgumentNullException(nameof(emailEncryption));
        }
    }
}
