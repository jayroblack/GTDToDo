using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Profile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Security
{
    public class ValidateEmailTokenServiceResult : IServiceResult
    {
        public bool IsValid { get; }

        public ValidateEmailTokenServiceResult(bool isValid)
        {
            IsValid = isValid;
        }
    }

    public class ValidateEmailTokenServicArgs : IServiceArgs<ValidateEmailTokenServiceResult>
    {
        public string Token { get; }

        public ValidateEmailTokenServicArgs(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException($"{nameof(token)} is required.");
            Token = token;
        }
    }

    public class
        ValidateEmailTokenService : IService<CreateValidateEmailTokenServiceArgs, ValidateEmailTokenServiceResult>
    {
        private readonly IQueryHandler<WhoAmIQueryArgs, WhoAmIQueryResult> _whoamId;

        public ValidateEmailTokenService(IQueryHandler<WhoAmIQueryArgs, WhoAmIQueryResult> whoamId)
        {
            _whoamId = whoamId ?? throw new ArgumentNullException(nameof(whoamId));
        }

        public async Task<ValidateEmailTokenServiceResult> Run(CreateValidateEmailTokenServiceArgs arg)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));
            var loggedInUser = await _whoamId.Run(new WhoAmIQueryArgs());

            //TODO:  Some Computer Science Cryptographic Signifigant operation
            return loggedInUser.Match<ValidateEmailTokenServiceResult>(
                some => new ValidateEmailTokenServiceResult(some.CurrentlyLoggedInUser.FirstName == arg.User.FirstName),
                () => new ValidateEmailTokenServiceResult(false));
        }
    }
}
