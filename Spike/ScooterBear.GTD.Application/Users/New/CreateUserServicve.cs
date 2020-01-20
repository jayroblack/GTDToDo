using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public class CreateUserServicve : IServiceAsync<CreateUserServiceArg, CreateUserServiceResult>
    {
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IServiceAsync<PersistNewUserServiceArgs, PersistNewUserServiceResult> _persistNewUserService;

        public CreateUserServicve(IKnowTheDate iKnowTheDate, ICreateIdsStrategy createIdsStrategy,
            IServiceAsync<PersistNewUserServiceArgs, PersistNewUserServiceResult> persistNewUserService)
        {
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _persistNewUserService =
                persistNewUserService ?? throw new ArgumentNullException(nameof(persistNewUserService));
        }

        public async Task<CreateUserServiceResult> Run(CreateUserServiceArg arg)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));

            var newUser = new NewUser(arg.Id, arg.FirstName, arg.LastName, arg.Email, _iKnowTheDate.UtcNow());

            var result =
                await _persistNewUserService.Run(new PersistNewUserServiceArgs(newUser));

            return new CreateUserServiceResult(result.ReadonlyUser);
        }
    }
}
