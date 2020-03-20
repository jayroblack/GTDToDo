using FluentAssertions;
using ScooterBear.GTD.Application.Services.Security;
using ScooterBear.GTD.Fakes;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Services
{
    public class AsTheEncryptDecryptStrategyI
    {
        [Fact]
        public void ShouldBeAbleToEncryptAndDecrypt()
        {
            var hardCoded = new HardCodedSecurityConfigurationFactory();
            var strategy = new EncryptDecryptStrategy(hardCoded);

            var toEncrypt = "This is my super secret content that nobody can ever know about you guys!!!!";

            (var iv, var encrypted) = strategy.Encrypt(toEncrypt);

            var unEncrypted = strategy.Decrypt(iv, encrypted);

            unEncrypted.Should().Be(toEncrypt);
        }
    }
}