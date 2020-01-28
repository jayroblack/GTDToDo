using System;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.Users.New;
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
            var id = "ID1";
            var name = "James";
            var last = "Rhodes";
            var email = "jayroblack@here.com";

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
                },
                outcome => outcome.Should().NotBe(outcome == CreateUserServiceOutcome.UserExists)
            );

            optionResult =
                await createUserService.Run(new CreateUserServiceArg(id, name, last, email));

            optionResult.Match(result => Assert.False(true, "ShouldFail"),
                outcome => outcome.Should().Be(CreateUserServiceOutcome.UserExists));
        }
    }
}
