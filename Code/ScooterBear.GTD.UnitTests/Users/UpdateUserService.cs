﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsTheUpdateUserServiceI : IClassFixture<UpdateUserServiceFixture>
    {
        public UpdateUserServiceFixture Fixture { get; }

        public AsTheUpdateUserServiceI(UpdateUserServiceFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldReturnNotFoundIfUserDoesNotExist()
        {
            this.Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                .Returns(Task.FromResult(Option.None<GetUserQueryResult>()));

            var result = await this.Fixture.UpdateUserService.Run(this.Fixture.UpdateUserServiceArgs);

            result.HasValue.Should().BeFalse("No Result was returned.");
            result.Match(
                some => Assert.True(false),
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldReturnNewVersionOnSuccess()
        {
            this.Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                .Returns(Task.FromResult(Option.Some<GetUserQueryResult>(new GetUserQueryResult(this.Fixture.User))));

            var optionSome =
                Option.Some<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                    new PersistUpdatedUserServiceResult(this.Fixture.User));

            this.Fixture.PersistUpdatedUser
                .Setup(x =>
                    x.Run(It.IsAny<PersistUpdatedUserServiceArgs>()))
                .Returns(Task.FromResult(optionSome));

            var result = await this.Fixture.UpdateUserService.Run(this.Fixture.UpdateUserServiceArgs);

            result.HasValue.Should().BeTrue("A value was found");
            result.Match(some => Assert.True(true), outcome => Assert.True(false));
        }

        [Fact]
        public async void IfPersistCausesConflictReturnConflict()
        {
            this.Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                .Returns(Task.FromResult(Option.Some<GetUserQueryResult>(new GetUserQueryResult(this.Fixture.User))));

            var optionNone =
                Option.None<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                    PersistUpdatedUserOutcome.Conflict);

            this.Fixture.PersistUpdatedUser
                .Setup(x =>
                    x.Run(It.IsAny<PersistUpdatedUserServiceArgs>()))
                .Returns(Task.FromResult(optionNone));

            var result = await this.Fixture.UpdateUserService.Run(this.Fixture.UpdateUserServiceArgs);

            result.HasValue.Should().BeFalse("Conflict");
            result.Match(some => Assert.True(false), outcome => Assert.True(true));
        }
    }

    public class UpdateUserServiceFixture : IDisposable
    {
        public readonly Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>> GetUser;

        public readonly Mock<IServiceOptOutcomes<PersistUpdatedUserServiceArgs,
            PersistUpdatedUserServiceResult,
            PersistUpdatedUserOutcome>> PersistUpdatedUser;

        public readonly UpdateUserService UpdateUserService;
        public readonly UpdateUserServiceArgs UpdateUserServiceArgs;
        public readonly User User;

        public UpdateUserServiceFixture()
        {
            this.GetUser = new Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>>();
            this.PersistUpdatedUser = new Mock<IServiceOptOutcomes<
                PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>>();
            this.UpdateUserService = new UpdateUserService(this.GetUser.Object, this.PersistUpdatedUser.Object);
            this.UpdateUserServiceArgs = new UpdateUserServiceArgs()
            {
                ID = "Id", AuthId = "AuthId", FirstName = "james", Email = "Email",
                BillingId = "BillingId", DateCreated = DateTime.Now, LastName = "LastName", VersionNumber = 4,
                IsEmailVerified = true
            };
            this.User = new User("Id", "FirstName", "LastName", "Email", true, "BillingId", "AuthId", 0, DateTime.Now);
        }

        public void Dispose()
        {

        }
    }
}
