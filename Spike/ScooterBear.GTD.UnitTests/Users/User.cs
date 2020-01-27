using System;
using FluentAssertions;
using ScooterBear.GTD.Application.Users.Update;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsAUserI
    {
        [Theory]
        [InlineData(null, "FirstName", "LastName", "Email")]
        [InlineData("Id", null, "LastName", "Email")]
        [InlineData("Id", "FirstName", null, "Email")]
        [InlineData("Id", "FirstName", "LastName", null)]
        public void ShouldThrowWhenArgumentIsNull(string id, string firstName, string lastName, string email)
        {
            Action toRun = () => new User(id, firstName, lastName, email, true, "BillingId", "AuthId", 1, DateTime.Now);
            toRun.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "AuthId")]
        [InlineData("BillingId", null)]
        [InlineData("BillingId", "AuthId")]
        public void ShouldNotThrowRegardless(string billingId, string authId)
        {
            Action toRun = () =>
                new User("Id", "FirstName", "LastName", "Email", true, billingId, authId, 1, DateTime.Now);
            toRun.Should().NotThrow<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowWhenVersionIsLessThan0()
        {
            Action toRun = () =>
                new User("Id", "FirstName", "LastName", "Email", true, "BillingId", "AuthId", -1, DateTime.Now);
            toRun.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowIfDateHasNotBeenSet()
        {
            Action toRun = () => new User("Id", "FirstName", "LastName", "Email", true, "BillingId", "AuthId", 0,
                DateTime.MinValue);
            toRun.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldMarkEmailUnVerifiedWhenChanged()
        {
            var user = new User("Id", "FirstName", "LastName", "Email", false, "BillingId", "AuthId", 0, DateTime.Now);

            user.VerifyEmail();
            user.IsEmailVerified.Should().BeTrue("We called VerifyEmail");
            user.SetEmail("NewEmail");
            user.IsEmailVerified.Should().BeFalse("We changed email, and it has not been verified.");

            user = new User("Id", "FirstName", "LastName", "Email", null, "BillingId", "AuthId", 0, DateTime.Now);

            user.VerifyEmail();
            user.IsEmailVerified.Should().BeTrue("We called VerifyEmail");
            user.SetEmail("NewEmail");
            user.IsEmailVerified.Should().BeFalse("We changed email, and it has not been verified.");
        }

        [Theory]
        [InlineData(false, true, true, false)]
        [InlineData(null, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, true, true, true)]
        public void ShouldOnlyMakeAccountEnabledWhen(bool? isEmailVerfied, bool billingIdSet, bool authIdIsSet,
            bool userEnabled)
        {
            var billingId = billingIdSet ? "BillingId" : null;
            var authId = authIdIsSet ? "AuthId" : null;

            var user = new User("Id", "FirstName", "LastName", "Email", isEmailVerfied, billingId, authId, 0,
                DateTime.Now);

            user.IsAccountEnabled.GetValueOrDefault().Should().Be(userEnabled);
        }
    }
}
