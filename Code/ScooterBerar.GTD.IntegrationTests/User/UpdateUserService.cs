﻿using System;
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
            var id = "ID2";
            var name = "James";
            var last = "Rhodes";
            var email = "jayroblack@here.com";

            var _createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                              CreateUserServiceResult, CreateUserServiceOutcome>>();

            var _updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var updateOption = await _updateUser.Run(new UpdateUserServiceArgs()
                {
                    ID = id,
                    FirstName = name,
                    LastName = last,
                    Email = email
                });

            updateOption.Match(
                some => Assert.True(false, "Should Fail"), 
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldBeUnprocessableWithRubbish()
        {
            var id = "ID2";
            var name = "James";
            var last = "Rhodes";
            var email = "jayroblack@here.com";

            var _createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            await _createUser.Run(new CreateUserServiceArg(id, name, last, email));

            var _updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var updateOption = await _updateUser.Run(new UpdateUserServiceArgs()
            {
                ID = id,
                FirstName = null,
                LastName = null,
                Email = null
            });

            updateOption.Match(
                some => Assert.True(false, "Should Fail"),
                outcome => outcome.Should().Be(UpdateUserService.UpdateUserOutcome.UnprocessableEntity));
        }

        [Fact]
        public async void ShouldUpdateValues()
        {
            var id = "ID2";
            var name = "James";
            var last = "Rhodes";
            var email = "jayroblack@here.com";

            var _createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            await _createUser.Run(new CreateUserServiceArg(id, name, last, email));

            var _updateUser = _fixture.Container.Resolve<IServiceOptOutcomes<UpdateUserServiceArgs,
                UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>>();

            var newFirstName = "Mc";
            var newLastName = "Tastey";
            var newEmail = "mcTastey@McLuvn";
            var isEmailverified = true;
            var billingId = "BillingId";
            var authId = "AuthId";

            var updateOption = await _updateUser.Run(new UpdateUserServiceArgs()
            {
                ID = id,
                FirstName = newFirstName,
                LastName = newLastName,
                Email = newEmail,
                IsEmailVerified = isEmailverified,
                BillingId = billingId,
                AuthId = authId
            });

            updateOption.Match(
                some =>
                {
                    var user = some.User;
                    user.FirstName.Should().Be(newFirstName);
                    user.LastName.Should().Be(newLastName);
                    user.Email.Should().Be(newEmail);
                    user.IsEmailVerified.Should().Be(true);
                    user.BillingId.Should().Be(billingId);
                    user.AuthId.Should().Be(authId);
                    user.IsAccountEnabled.Should().Be(true);
                },
                outcome => Assert.False(true, "Should Have Succeeded"));
        }
    }
}