using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.User
{
    [Collection("DynamoDbDockerTests")]
    public class AsAGetOrCreateUserServiceI
    {
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsAGetOrCreateUserServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldCreateUserIfDoesNotExist()
        {
            var id = _fixture.Container.Resolve<ICreateIdsStrategy>().NewId();
            var name = "James";
            var last = "Blah";
            var email = $"jayroblack+{id}@here.com";


            var getUserQuery = _fixture.Container.Resolve<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>>();

            var queryResult = await getUserQuery.Run(new GetUserQueryArgs(id));
            queryResult.Match(some => Assert.False(true), () => Assert.True(true));

            var getOrCreateUserService = _fixture.Container
                .Resolve<IServiceOpt<GetOrCreateUserServiceArgs, GetOrCreateUserServiceResult>>();

            var resultOption = 
                await getOrCreateUserService.Run(new GetOrCreateUserServiceArgs(id, name, last, email));

            resultOption.Match(some =>
            {
                var user = some.User;
                user.ID.Should().Be(id);
                user.FirstName.Should().Be(name);
                user.LastName.Should().Be(last);
                user.Email.Should().Be(email);
            }, 
                () => Assert.False(true));

            queryResult = await getUserQuery.Run(new GetUserQueryArgs(id));
            queryResult.Match(some => Assert.True(true), () => Assert.False(true));
        }

        [Fact]
        public async void ShouldReturnExistingUserIfExists()
        {
            var id = _fixture.Container.Resolve<ICreateIdsStrategy>().NewId();
            var name = "James";
            var last = "Blah";
            var email = $"jayroblack+{id}@here.com";

            var createUser = _fixture.Container.Resolve<IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome>>();

            var resultOption = await createUser.Run(new CreateUserServiceArg(id, name, last, email));
            resultOption.HasValue.Should().BeTrue();

            var getOrCreateUserService = _fixture.Container
                .Resolve<IServiceOpt<GetOrCreateUserServiceArgs, GetOrCreateUserServiceResult>>();

            var getOrCreateResultOption =
                await getOrCreateUserService.Run(new GetOrCreateUserServiceArgs(id, name, last, email));

            getOrCreateResultOption.Match(some =>
                {
                    var user = some.User;
                    user.ID.Should().Be(id);
                    user.FirstName.Should().Be(name);
                    user.LastName.Should().Be(last);
                    user.Email.Should().Be(email);
                },
                () => Assert.False(true));

        }
    }
}
