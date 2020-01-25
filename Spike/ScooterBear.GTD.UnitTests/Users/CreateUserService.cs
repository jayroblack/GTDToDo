using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsACreateUserServiceI : IClassFixture<CreateUserFixture>
    {
        public CreateUserFixture CreateUserFixture { get; }

        public AsACreateUserServiceI(CreateUserFixture createUserFixture)
        {
            CreateUserFixture = createUserFixture ?? throw new ArgumentNullException(nameof(createUserFixture));
        }

        [Fact]
        public async void ShouldReturnUserExistsIfUserFound()
        {
            this.CreateUserFixture.GetUserService.Setup(x => 
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                    .Returns(Task.FromResult(Option.Some(new GetUserQueryResult(this.CreateUserFixture.User))));

            var result = await
                this.CreateUserFixture.CreateUserService.Run(new CreateUserServiceArg("Id", "FirstName", "LastName",
                    "Email"));

            result.HasValue.Should().BeFalse("A user exists");
            result.MatchNone((outcome => outcome.Should().Be(CreateUserServiceOutcome.UserExists)));
            result.MatchSome(some=> Assert.False(true));
        }

        [Fact]
        public async void ShouldSaveUserIfNotFoundAndReturnNewUser()
        {
            this.CreateUserFixture.GetUserService.Setup(x => 
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                    .Returns(Task.FromResult(Option.None<GetUserQueryResult>()));

            this.CreateUserFixture.PersistNewUserService.Setup(x =>
                    x.Run(It.IsAny<PersistNewUserServiceArgs>()))
                .Returns(Task.FromResult(new PersistNewUserServiceResult(this.CreateUserFixture.User)));

            this.CreateUserFixture.IKnowTheDate.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            var result = await
                this.CreateUserFixture.CreateUserService.Run(new CreateUserServiceArg("Id", "FirstName", "LastName",
                    "Email"));

            result.HasValue.Should().BeTrue("The persist operation succeeded and we fetched the fresh version.");
            result.MatchSome(some=> Assert.True(true));
            result.MatchNone(outcome => Assert.True(false));
        }
    }

    public class CreateUserFixture : IDisposable
    {
        public Mock<IKnowTheDate> IKnowTheDate;
        public Mock<ICreateIdsStrategy> CreateIdsStrategy;
        public Mock<IService<PersistNewUserServiceArgs, PersistNewUserServiceResult>> PersistNewUserService;
        public Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>> GetUserService;
        public CreateUserService CreateUserService;
        public User User;

        public CreateUserFixture()
        {
            this.IKnowTheDate = new Mock<IKnowTheDate>();
            this.CreateIdsStrategy = new Mock<ICreateIdsStrategy>();
            this.PersistNewUserService =
                new Mock<IService<PersistNewUserServiceArgs, PersistNewUserServiceResult>>();
            this.GetUserService = new Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>>();
            this.CreateUserService = new CreateUserService(IKnowTheDate.Object, CreateIdsStrategy.Object,
                PersistNewUserService.Object, GetUserService.Object);
            this.User = new User("Id", "FirstName", "LastName", "Email", true, "BillingId", "AuthId", 0, DateTime.Now);
        }

        public void Dispose()
        {

        }
    }
}
