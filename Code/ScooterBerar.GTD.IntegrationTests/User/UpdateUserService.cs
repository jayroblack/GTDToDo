using System;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.User
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheUpdateUserServiceI
    {
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsTheUpdateUserServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldDetectIfItemDoesNotExist()
        {
            var user = _fixture.GenerateUser();

            var updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var updateOption = await updateUser.Run(new UpdateUserServiceArgs(user.ID, user.FirstName, user.LastName,
                user.Email, user.BillingId, user.AuthId, user.VersionNumber));
            
            updateOption.Match(
                some => Assert.True(false, "Should Fail"), 
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldBeUnprocessableWithRubbish()
        {
            var user = _fixture.GenerateUser();

            var createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            await createUser.Run(new CreateUserServiceArg(user.ID, user.FirstName, user.LastName, user.Email));

            var updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var updateOption = await updateUser.Run(new UpdateUserServiceArgs(user.ID, null, null,
                null, user.BillingId, user.AuthId, user.VersionNumber));

            updateOption.Match(
                some => Assert.True(false, "Should Fail"),
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.UnprocessableEntity));
        }

        [Fact]
        public async void ShouldUpdateValues()
        {
            var user = _fixture.GenerateUser();

            var createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            var createUserOptional = 
                await createUser.Run(new CreateUserServiceArg(user.ID, user.FirstName, user.LastName, user.Email));
            createUserOptional.HasValue.Should().BeTrue();

            var existingVeresionNumber = 0;
            createUserOptional.MatchSome(x=> existingVeresionNumber = x.User.VersionNumber);

            var _updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var billingId = "NewBillingId";
            var authId = "NewAuthId";

            var updateOption = await _updateUser.Run(new UpdateUserServiceArgs(user.ID, newFirstName, newLastName,
                newEmail, billingId, authId, existingVeresionNumber));

            updateOption.Match(
                some =>
                {
                    var user = some.User;
                    user.FirstName.Should().Be(newFirstName);
                    user.LastName.Should().Be(newLastName);
                    user.Email.Should().Be(newEmail);
                    user.BillingId.Should().Be(billingId);
                    user.AuthId.Should().Be(authId);
                    user.IsAccountEnabled.Should().Be(true);
                },
                outcome => Assert.False(true, "Should Have Succeeded"));
        }

        [Fact]
        public async void ShouldReturnVersionConflict()
        {
            var user = _fixture.GenerateUser();

            var createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            await createUser.Run(new CreateUserServiceArg(user.ID, user.FirstName, user.LastName, user.Email));

            var _updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var billingId = "NewBillingId";
            var authId = "NewAuthId";

            var updateOption = await _updateUser.Run(new UpdateUserServiceArgs(user.ID, newFirstName, newLastName,
                newEmail, billingId, authId, 100));

            updateOption.Match(
                some =>
                Assert.False(true, "Should not pass."),
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.VersionConflict));
        }
    }
}
