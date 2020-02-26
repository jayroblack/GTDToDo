using System;
using System.Threading.Tasks;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;
using FluentAssertions;

namespace ScooterBear.GTD.UnitTests.Users
{
    public class AsAGetOrCreateUserServiceI : IClassFixture<GetOrCreateUserFixture>
    {
        public GetOrCreateUserFixture Fixture { get; }

        public AsAGetOrCreateUserServiceI(GetOrCreateUserFixture fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldCreateNewUserIfNotFound()
        {
            this.Fixture.GetUserMock.Setup(x =>
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                .Returns(Task.FromResult(
                    Option.None<GetUserQueryResult>()));

            this.Fixture.CreateUserServiceMock
                .Setup(x =>
                    x.Run(It.IsAny<CreateUserServiceArg>()))
                .Returns(Task.FromResult(Option.Some<CreateUserServiceResult, CreateUserServiceOutcome>(new CreateUserServiceResult(this.Fixture.User))));

            var resultOption = await 
                this.Fixture.GetOrCreateUserService.Run(new GetOrCreateUserServiceArgs("Id", "FirstName", "LastName",
                    "Email"));

            resultOption.HasValue.Should().BeTrue();
            this.Fixture.GetUserMock.Verify(x=> x.Run(It.IsAny<GetUserQueryArgs>()));
            this.Fixture.GetUserMock.VerifyNoOtherCalls();
            this.Fixture.CreateUserServiceMock.Verify(x => x.Run(It.IsAny<CreateUserServiceArg>()));
            this.Fixture.CreateUserServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldReturnExistingUserIfFound()
        {
            this.Fixture.GetUserMock.Setup(x =>
                    x.Run(It.IsAny<GetUserQueryArgs>()))
                .Returns(Task.FromResult(
                    Option.Some(
                        new GetUserQueryResult(
                            this.Fixture.User))));

            var resultOption = await
                this.Fixture.GetOrCreateUserService.Run(new GetOrCreateUserServiceArgs("Id", "FirstName", "LastName",
                    "Email"));

            resultOption.HasValue.Should().BeTrue();
            this.Fixture.GetUserMock.Verify(x => x.Run(It.IsAny<GetUserQueryArgs>()));
            this.Fixture.GetUserMock.VerifyNoOtherCalls();
            this.Fixture.CreateUserServiceMock.VerifyNoOtherCalls();
        }

    }

    public class GetOrCreateUserFixture : IDisposable
    {

        public Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>> GetUserMock;
        public Mock<IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome>> CreateUserServiceMock;
        public GetOrCreateUserService GetOrCreateUserService;
        public IUser User;

        public GetOrCreateUserFixture()
        {
            this.GetUserMock = new Mock<IQueryHandler<GetUserQueryArgs, GetUserQueryResult>>();
            this.CreateUserServiceMock = new Mock<IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome>>();
            this.User = new User("Id", "firstName", "lastName", "email", "billingId", "authId", 0, DateTime.UtcNow);
            this.GetOrCreateUserService = new GetOrCreateUserService(GetUserMock.Object, CreateUserServiceMock.Object);
        }

        public void Dispose()
        {
            
        }
    }
}
