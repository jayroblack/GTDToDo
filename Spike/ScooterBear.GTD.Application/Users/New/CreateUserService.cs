using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public enum CreateUserServiceOutcome
    {
        UserExists
    }

    public class CreateUserService : IServiceAsyncOptionalOutcomes<CreateUserServiceArg,
        CreateUserServiceResult, CreateUserServiceOutcome>
    {
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IServiceAsync<PersistNewUserServiceArgs, PersistNewUserServiceResult> _persistNewUserService;
        private readonly IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> _getUser;

        public CreateUserService(IKnowTheDate iKnowTheDate, 
            ICreateIdsStrategy createIdsStrategy,
            IServiceAsync<PersistNewUserServiceArgs, PersistNewUserServiceResult> persistNewUserService,
            IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> getUser)
        {
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _persistNewUserService =
                persistNewUserService ?? throw new ArgumentNullException(nameof(persistNewUserService));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
        }

        public async Task<Option<CreateUserServiceResult, CreateUserServiceOutcome>> Run(CreateUserServiceArg arg)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));

            var queryOption = await _getUser.Run(new GetUserQueryArgs(arg.Id));

            //TODO:  Come back and make this async!!!!
            return
                queryOption.Match(
                    some => Option.None<CreateUserServiceResult, CreateUserServiceOutcome>(
                        CreateUserServiceOutcome.UserExists),
                    () =>
                    {
                        var newUser = new NewUser(arg.Id, arg.FirstName, arg.LastName, arg.Email,
                            _iKnowTheDate.UtcNow());

                        //SHOULD BE ASYNC!!!!
                        var result =
                            _persistNewUserService.Run(new PersistNewUserServiceArgs(newUser)).Result;

                        return Option.Some<CreateUserServiceResult, CreateUserServiceOutcome>(
                            new CreateUserServiceResult(result.ReadonlyUser));
                    });
        }
    }
}
