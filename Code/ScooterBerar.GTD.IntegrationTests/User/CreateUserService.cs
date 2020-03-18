﻿using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.User
{
    [Collection("DynamoDbDockerTests")]
    public class AsACreateUserServiceI
    {
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsACreateUserServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldSaveNewUser()
        {
            var id = _fixture.Container.Resolve<ICreateIdsStrategy>().NewId();
            var userId = _fixture.Container.Resolve<ICreateIdsStrategy>().NewId();
            var name = "James";
            var last = "Blah";
            var email = $"jayroblack+{id}@here.com";
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserService = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome>>();

            var optionResult =
                await createUserService.Run(new CreateUserServiceArg(id, name, last, email));

            optionResult.Match(result =>
                {
                    var createdUser = result.User;
                    createdUser.ID.Should().Be(id);
                    createdUser.FirstName.Should().Be(name);
                    createdUser.LastName.Should().Be(last);
                    createdUser.Email.Should().Be(email);
                    createdUser.IsAccountEnabled.GetValueOrDefault().Should().BeFalse();
                },
                outcome => outcome.Should().NotBe(outcome == CreateUserServiceOutcome.UserExists)
            );

            optionResult =
                await createUserService.Run(new CreateUserServiceArg(id, name, last, email));

            optionResult.Match(result => Assert.False(true, "ShouldFail"),
                outcome => outcome.Should().Be(CreateUserServiceOutcome.UserExists));
        }

        [Fact]
        public async void ShouldCreateDefaultProjectInbox()
        {
            var userId = _fixture.Container.Resolve<ICreateIdsStrategy>().NewId();
            var name = "James3";
            var last = "Blah3";
            var email = $"jayroblack+{userId}@here.com";
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserService = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome>>();

            var optionResult =
                await createUserService.Run(new CreateUserServiceArg(userId, name, last, email));

            optionResult.HasValue.Should().BeTrue();

            var getProjectsQuery =
                _fixture.Container.Resolve<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>>();

            var result = await getProjectsQuery.Run(new GetUserProjectsQuery(userId));

            result.HasValue.Should().BeTrue();
            result.MatchSome(some =>
            {
                some.UserProjects.Should().NotBeNull();
                some.UserProjects.Projects.Should().NotBeNull();
                some.UserProjects.Projects.Count().Should().Be(1);

                var defaultProject = some.UserProjects.Projects.First();
                defaultProject.Name.Should().Be("Inbox");
            });
        }
    }
}
