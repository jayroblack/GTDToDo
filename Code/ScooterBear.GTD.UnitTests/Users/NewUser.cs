using System;
using FluentAssertions;
using ScooterBear.GTD.Application.Users.New;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsANewUserI
    {
        [Theory]
        [InlineData(null, "FirsdtName", "LastName", "Email")]
        [InlineData("Id", null, "LastName", "Email")]
        [InlineData("Id", "FirsdtName", null, "Email")]
        [InlineData("Id", "FirsdtName", "LastName", null)]
        public void ShouldThrowWhenArgumentNull(string id, string firstName, string lastName, string email)
        {
            Action toRun = () => new NewUser(id, firstName, lastName, email, DateTime.Now);
            toRun.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ShouldThrowWhenDateIsNotSet()
        {
            Action toRun = () => new NewUser("Id", "FirstName", "LastName", "Email", default);
            toRun.Should().Throw<ArgumentException>();
        }
    }
}