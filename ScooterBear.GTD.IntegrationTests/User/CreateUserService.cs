using System;
using Autofac;
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
        public async void TestFact()
        {
            var createUserService = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome>>();
        }
    }
}
