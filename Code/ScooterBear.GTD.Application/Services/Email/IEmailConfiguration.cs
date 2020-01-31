using System;

namespace ScooterBear.GTD.Application.Services.Email
{
    public interface IEmailConfigurationFactory
    {
        EmailConfiguration Create();
    }

    public class EmailConfiguration
    {
        public string FromEmailAddress { get; }
        public string ConfigSetName { get; }

        public EmailConfiguration(string fromEmailAddress, string configSetName)
        {
            if( string.IsNullOrEmpty(fromEmailAddress))
                throw new ArgumentException($"{nameof(fromEmailAddress)} is required.");

            FromEmailAddress = fromEmailAddress;
            ConfigSetName = configSetName;
        }
    }
}
