using System;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users
{
    public class GetOrCreateUserService : IServiceOpt<GetOrCreateUserArg, GetOrCreateUserResult>
    {
        private readonly IServiceOpt<CreateUserArg, CreateUserResult, CreateUserServiceOutcome>
            _createUserService;

        private readonly IQueryHandler<GetUserArg, GetUserQueryResult> _getUser;

        public GetOrCreateUserService(IQueryHandler<GetUserArg, GetUserQueryResult> getUser,
            IServiceOpt<CreateUserArg,
                CreateUserResult, CreateUserServiceOutcome> createUserService)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _createUserService = createUserService ?? throw new ArgumentNullException(nameof(createUserService));
        }

        public async Task<Option<GetOrCreateUserResult>> Run(GetOrCreateUserArg arg)
        {
            var resultOption = await _getUser.Run(new GetUserArg(arg.Id));

            return await resultOption.MatchAsync(async some => Option.Some(new GetOrCreateUserResult(some.User)),
                async () =>
                {
                    var createOptionResult =
                        await _createUserService.Run(new CreateUserArg(arg.Id, arg.FirstName, arg.LastName, arg.Email));

                    return createOptionResult.Match(
                        innerSome => Option.Some(new GetOrCreateUserResult(innerSome.User)),
                        outcome => Option.None<GetOrCreateUserResult>());
                });
        }
    }
}