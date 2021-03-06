﻿using System;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.User
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheUpdateUserServiceI
    {
        public AsTheUpdateUserServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly RunDynamoDbDockerFixture _fixture;

        [Fact]
        public async void ShouldBeUnprocessableWithRubbish()
        {
            var user = _fixture.GenerateUser();
            _fixture.ProfileFactory.SetUserProfile(new Profile(user.ID));

            var createUser = _fixture.Container.Resolve<IServiceOpt<CreateUserArg,
                CreateUserResult, CreateUserServiceOutcome>>();

            await createUser.Run(new CreateUserArg(user.ID, user.FirstName, user.LastName, user.Email));

            var updateUser = _fixture.Container.Resolve<IServiceOpt<UpdateUserArg,
                UpdateUserResult, UpdateUserOutcome>>();

            var updateOption = await updateUser.Run(new UpdateUserArg(user.ID, null, null,
                null, user.BillingId, user.AuthId, user.VersionNumber));

            updateOption.Match(
                some => Assert.True(false, "Should Fail"),
                outcome => outcome.Should().Be(UpdateUserOutcome.UnprocessableEntity));
        }

        [Fact]
        public async void ShouldDetectIfItemDoesNotExist()
        {
            var user = _fixture.GenerateUser();
            _fixture.ProfileFactory.SetUserProfile(new Profile(user.ID));
            var updateUser = _fixture.Container.Resolve<IServiceOpt<UpdateUserArg,
                UpdateUserResult, UpdateUserOutcome>>();

            var updateOption = await updateUser.Run(new UpdateUserArg(user.ID, user.FirstName, user.LastName,
                user.Email, user.BillingId, user.AuthId, user.VersionNumber));

            updateOption.Match(
                some => Assert.True(false, "Should Fail"),
                outcome => outcome.Should().Be(UpdateUserOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfUserDoesNotMatchProfile()
        {
            var user = _fixture.GenerateUser();
            _fixture.ProfileFactory.SetUserProfile(new Profile(user.ID));
            var createUser = _fixture.Container.Resolve<IServiceOpt<CreateUserArg,
                CreateUserResult, CreateUserServiceOutcome>>();

            var createUserOptional =
                await createUser.Run(new CreateUserArg(user.ID, user.FirstName, user.LastName, user.Email));
            createUserOptional.HasValue.Should().BeTrue();

            var existingVeresionNumber = 0;
            createUserOptional.MatchSome(x => existingVeresionNumber = x.User.VersionNumber);

            _fixture.ProfileFactory.SetUserProfile(new Profile(Guid.NewGuid().ToString()));
            var _updateUser = _fixture.Container.Resolve<IServiceOpt<UpdateUserArg,
                UpdateUserResult, UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var billingId = "NewBillingId";
            var authId = "NewAuthId";

            var updateOption = await _updateUser.Run(new UpdateUserArg(user.ID, newFirstName, newLastName,
                newEmail, billingId, authId, existingVeresionNumber));

            updateOption.Match(
                some =>
                    Assert.False(true, "Should not pass."),
                outcome => outcome.Should().Be(UpdateUserOutcome.NotAuthorized));
        }

        [Fact]
        public async void ShouldReturnVersionConflictIfModifyingStaleRead()
        {
            var user = _fixture.GenerateUser();
            _fixture.ProfileFactory.SetUserProfile(new Profile(user.ID));
            var createUser = _fixture.Container.Resolve<IServiceOpt<CreateUserArg,
                CreateUserResult, CreateUserServiceOutcome>>();

            await createUser.Run(new CreateUserArg(user.ID, user.FirstName, user.LastName, user.Email));

            var _updateUser = _fixture.Container.Resolve<IServiceOpt<UpdateUserArg,
                UpdateUserResult, UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var billingId = "NewBillingId";
            var authId = "NewAuthId";

            var updateOption = await _updateUser.Run(new UpdateUserArg(user.ID, newFirstName, newLastName,
                newEmail, billingId, authId, 100));

            updateOption.Match(
                some =>
                    Assert.False(true, "Should not pass."),
                outcome => outcome.Should().Be(UpdateUserOutcome.VersionConflict));
        }

        [Fact]
        public async void ShouldUpdateValues()
        {
            var user = _fixture.GenerateUser();
            _fixture.ProfileFactory.SetUserProfile(new Profile(user.ID));
            var createUser = _fixture.Container.Resolve<IServiceOpt<CreateUserArg,
                CreateUserResult, CreateUserServiceOutcome>>();

            var createUserOptional =
                await createUser.Run(new CreateUserArg(user.ID, user.FirstName, user.LastName, user.Email));
            createUserOptional.HasValue.Should().BeTrue();

            var existingVeresionNumber = 0;
            createUserOptional.MatchSome(x => existingVeresionNumber = x.User.VersionNumber);

            var _updateUser = _fixture.Container.Resolve<IServiceOpt<UpdateUserArg,
                UpdateUserResult, UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var billingId = "NewBillingId";
            var authId = "NewAuthId";

            var updateOption = await _updateUser.Run(new UpdateUserArg(user.ID, newFirstName, newLastName,
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
    }
}