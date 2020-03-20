using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optional;
using ScooterBear.GTD.Application;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Fakes;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsTheUpdateUserServiceI : IClassFixture<UpdateUserServiceFixture>
    {
        public AsTheUpdateUserServiceI(UpdateUserServiceFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        public UpdateUserServiceFixture Fixture { get; }

        [Fact]
        public async void IfPersistCausesConflictReturnConflict()
        {
            Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(Option.Some(new GetUserQueryResult(Fixture.User))));

            var optionNone =
                Option.None<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                    PersistUpdatedUserOutcome.Conflict);

            Fixture.PersistUpdatedUser
                .Setup(x =>
                    x.Run(It.IsAny<PersistUpdatedUserServiceArgs>()))
                .Returns(Task.FromResult(optionNone));

            var result = await Fixture.UpdateUserService.Run(Fixture.UpdateUserArg);

            result.HasValue.Should().BeFalse("Conflict");
            result.Match(some => Assert.True(false), outcome => Assert.True(true));
        }

        [Fact]
        public async void IfUserProfileDoesNotMatchDataReturnUnAuthorized()
        {
            Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(Option.Some(new GetUserQueryResult(Fixture.User))));

            var optionSome =
                Option.Some<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                    new PersistUpdatedUserServiceResult(Fixture.User));

            Fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(Guid.NewGuid().ToString()));
            Fixture.PersistUpdatedUser
                .Setup(x =>
                    x.Run(It.IsAny<PersistUpdatedUserServiceArgs>()))
                .Returns(Task.FromResult(optionSome));

            var result = await Fixture.UpdateUserService.Run(Fixture.UpdateUserArg);

            result.Match(some => Assert.False(true, "Should return Unauthorized"),
                outcome => outcome.Should().Be(UpdateUserOutcome.NotAuthorized));
        }

        [Fact]
        public async void ShouldReturnNotFoundIfUserDoesNotExist()
        {
            Fixture.GetUser.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(Option.None<GetUserQueryResult>()));

            var result = await Fixture.UpdateUserService.Run(Fixture.UpdateUserArg);

            result.HasValue.Should().BeFalse("No Result was returned.");
            result.Match(
                some => Assert.True(false),
                outcome => outcome.Should().Be(UpdateUserOutcome.DoesNotExist));
        }
    }

    public class UpdateUserServiceFixture : IDisposable
    {
        public readonly Mock<IQueryHandler<GetUserArg, GetUserQueryResult>> GetUser;

        public readonly Mock<ILogger<UpdateUserService>> Logger;

        public readonly Mock<IServiceOptOutcomes<PersistUpdatedUserServiceArgs,
            PersistUpdatedUserServiceResult,
            PersistUpdatedUserOutcome>> PersistUpdatedUser;

        public readonly FakedProfileFactory ProfileFactory;
        public readonly UpdateUserArg UpdateUserArg;
        public readonly UpdateUserService UpdateUserService;
        public readonly User User;

        public UpdateUserServiceFixture()
        {
            GetUser = new Mock<IQueryHandler<GetUserArg, GetUserQueryResult>>();
            PersistUpdatedUser = new Mock<IServiceOptOutcomes<
                PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>>();
            var id = new GuidCreateIdStrategy().NewId();
            ProfileFactory = new FakedProfileFactory();
            var profile = new Application.UserProfile.Profile(id);
            ProfileFactory.SetUserProfile(profile);
            Logger = new Mock<ILogger<UpdateUserService>>();
            UpdateUserService =
                new UpdateUserService(ProfileFactory, Logger.Object, GetUser.Object, PersistUpdatedUser.Object);

            var firstName = "FirstName";
            var lastName = "LastName";
            var email = "Email";
            var billingId = "BilllingId";
            var versionNumber = 4;
            var authId = "AuthId";

            UpdateUserArg = new UpdateUserArg(id, firstName, lastName, email,
                billingId, authId, versionNumber);

            User = new User(id, firstName, lastName, email, billingId, authId, versionNumber, DateTime.Now);
        }

        public void Dispose()
        {
        }
    }
}