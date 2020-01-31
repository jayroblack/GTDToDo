using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public enum CreateUserServiceOutcome
    {
        UserExists
    }

    public class CreateUserService : IServiceOptOutcomes<CreateUserServiceArg,
        CreateUserServiceResult, CreateUserServiceOutcome>
    {
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IService<PersistNewUserServiceArgs, PersistNewUserServiceResult> _persistNewUserService;
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;

        public CreateUserService(IKnowTheDate iKnowTheDate,
            ICreateIdsStrategy createIdsStrategy,
            IService<PersistNewUserServiceArgs, PersistNewUserServiceResult> persistNewUserService,
            IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser)
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
                        //TODO:  Should User Names Be Unique?  -> Seems like that is an IDP Problem
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
