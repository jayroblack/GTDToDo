using System;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
{
    public class GetOrCreateUserService : IServiceOpt<GetOrCreateUserServiceArgs, GetOrCreateUserServiceResult>
    {
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;
        private readonly IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome> _createUserService;

        public GetOrCreateUserService(IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceOptOutcomes<CreateUserServiceArg,
                CreateUserServiceResult, CreateUserServiceOutcome> createUserService)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _createUserService = createUserService ?? throw new ArgumentNullException(nameof(createUserService));
        }

        public async Task<Option<GetOrCreateUserServiceResult>> Run(GetOrCreateUserServiceArgs arg)
        {
            var resultOption = await _getUser.Run(new GetUserQueryArgs(arg.Id));

            return await resultOption.MatchAsync(async some => Option.Some(new GetOrCreateUserServiceResult(some.User)),
                async () =>
                {
                    var createOptionResult =
                        await _createUserService.Run(new CreateUserServiceArg(arg.Id, arg.FirstName, arg.LastName, arg.Email));

                    return createOptionResult.Match<Option<GetOrCreateUserServiceResult>>(
                        innerSome => Option.Some(new GetOrCreateUserServiceResult(innerSome.User)),
                        outcome => Option.None<GetOrCreateUserServiceResult>());
                });
        }
    }
}
