using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using ScooterBear.GTD.Patterns.Domain;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsACreateUserServiceI : IClassFixture<CreateUserFixture>
    {
        public AsACreateUserServiceI(CreateUserFixture createUserFixture)
        {
            CreateUserFixture = createUserFixture ?? throw new ArgumentNullException(nameof(createUserFixture));
        }

        public CreateUserFixture CreateUserFixture { get; }

        [Fact]
        public async void ShouldReturnUserExistsIfUserFound()
        {
            CreateUserFixture.GetUserService.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(Option.Some(new GetUserQueryResult(CreateUserFixture.User))));

            var result = await
                CreateUserFixture.CreateUserService.Run(new CreateUserArg("Id", "FirstName", "LastName",
                    "Email"));

            result.HasValue.Should().BeFalse("A user exists");
            result.MatchNone(outcome => outcome.Should().Be(CreateUserServiceOutcome.UserExists));
            result.MatchSome(some => Assert.False(true));
        }

        [Fact]
        public async void ShouldSaveUserIfNotFoundAndReturnNewUser()
        {
            CreateUserFixture.GetUserService.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(Option.None<GetUserQueryResult>()));

            CreateUserFixture.PersistNewUserService.Setup(x =>
                    x.Run(It.IsAny<PersistNewUserArg>()))
                .Returns(Task.FromResult(new PersistNewUserResult(CreateUserFixture.User)));

            CreateUserFixture.IKnowTheDate.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            var result = await
                CreateUserFixture.CreateUserService.Run(new CreateUserArg("Id", "FirstName", "LastName",
                    "Email"));

            result.HasValue.Should().BeTrue("The persist operation succeeded and we fetched the fresh version.");
            result.MatchSome(some => Assert.True(true));
            result.MatchNone(outcome => Assert.True(false));
        }
    }

    public class CreateUserFixture : IDisposable
    {
        public Mock<ICreateIdsStrategy> CreateIdsStrategy;
        public CreateUserService CreateUserService;
        public Mock<IQueryHandler<GetUserArg, GetUserQueryResult>> GetUserService;
        public Mock<IKnowTheDate> IKnowTheDate;
        public Mock<IDomainEventHandlerStrategyAsync<NewUserCreatedEvent>> NewUserCreated;
        public Mock<IService<PersistNewUserArg, PersistNewUserResult>> PersistNewUserService;
        public User User;

        public CreateUserFixture()
        {
            IKnowTheDate = new Mock<IKnowTheDate>();
            CreateIdsStrategy = new Mock<ICreateIdsStrategy>();
            PersistNewUserService =
                new Mock<IService<PersistNewUserArg, PersistNewUserResult>>();
            GetUserService = new Mock<IQueryHandler<GetUserArg, GetUserQueryResult>>();
            NewUserCreated = new Mock<IDomainEventHandlerStrategyAsync<NewUserCreatedEvent>>();
            CreateUserService = new CreateUserService(IKnowTheDate.Object, CreateIdsStrategy.Object,
                PersistNewUserService.Object, GetUserService.Object, NewUserCreated.Object);
            User = new User("Id", "FirstName", "LastName", "Email", "BillingId", "AuthId", 0, DateTime.Now);
        }

        public void Dispose()
        {
        }
    }
}