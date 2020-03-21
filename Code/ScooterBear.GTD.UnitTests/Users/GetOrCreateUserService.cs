using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsAGetOrCreateUserServiceI : IClassFixture<GetOrCreateUserFixture>
    {
        public AsAGetOrCreateUserServiceI(GetOrCreateUserFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        public GetOrCreateUserFixture Fixture { get; }

        [Fact]
        public async void ShouldCreateNewUserIfNotFound()
        {
            Fixture.GetUserMock.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(
                    Option.None<GetUserQueryResult>()));

            Fixture.CreateUserServiceMock
                .Setup(x =>
                    x.Run(It.IsAny<CreateUserArg>()))
                .Returns(Task.FromResult(
                    Option.Some<CreateUserResult, CreateUserServiceOutcome>(new CreateUserResult(Fixture.User))));

            var resultOption = await
                Fixture.GetOrCreateUserService.Run(new GetOrCreateUserArg("Id", "FirstName", "LastName",
                    "Email"));

            resultOption.HasValue.Should().BeTrue();
            Fixture.GetUserMock.Verify(x => x.Run(It.IsAny<GetUserArg>()));
            Fixture.GetUserMock.VerifyNoOtherCalls();
            Fixture.CreateUserServiceMock.Verify(x => x.Run(It.IsAny<CreateUserArg>()));
            Fixture.CreateUserServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldReturnExistingUserIfFound()
        {
            Fixture.GetUserMock.Setup(x =>
                    x.Run(It.IsAny<GetUserArg>()))
                .Returns(Task.FromResult(
                    Option.Some(
                        new GetUserQueryResult(
                            Fixture.User))));

            var resultOption = await
                Fixture.GetOrCreateUserService.Run(new GetOrCreateUserArg("Id", "FirstName", "LastName",
                    "Email"));

            resultOption.HasValue.Should().BeTrue();
            Fixture.GetUserMock.Verify(x => x.Run(It.IsAny<GetUserArg>()));
            Fixture.GetUserMock.VerifyNoOtherCalls();
            Fixture.CreateUserServiceMock.VerifyNoOtherCalls();
        }
    }

    public class GetOrCreateUserFixture : IDisposable
    {
        public Mock<IServiceOpt<CreateUserArg, CreateUserResult, CreateUserServiceOutcome>>
            CreateUserServiceMock;

        public GetOrCreateUserService GetOrCreateUserService;

        public Mock<IQueryHandler<GetUserArg, GetUserQueryResult>> GetUserMock;
        public IUser User;

        public GetOrCreateUserFixture()
        {
            GetUserMock = new Mock<IQueryHandler<GetUserArg, GetUserQueryResult>>();
            CreateUserServiceMock =
                new Mock<IServiceOpt<CreateUserArg, CreateUserResult, CreateUserServiceOutcome>>();
            User = new User("Id", "firstName", "lastName", "email", "billingId", "authId", 0, DateTime.UtcNow);
            GetOrCreateUserService = new GetOrCreateUserService(GetUserMock.Object, CreateUserServiceMock.Object);
        }

        public void Dispose()
        {
        }
    }
}