using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Security
{
    public class CreateValidateEmailTokenServiceResult : IServiceResult
    {
        public string Token { get; }

        public CreateValidateEmailTokenServiceResult(string token)
        {
            if( string.IsNullOrEmpty(token))
                throw new ArgumentException($"{nameof(token)} is required.");
            Token = token;
        }
    }

    public class CreateValidateEmailTokenServiceArgs : IServiceArgs<CreateValidateEmailTokenServiceResult>, IServiceArgs<ValidateEmailTokenServiceResult>
    {
        public IUser User { get; }

        public CreateValidateEmailTokenServiceArgs(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class CreateValidateEmailTokenService : IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult>
    {
        public async Task<CreateValidateEmailTokenServiceResult> Run(CreateValidateEmailTokenServiceArgs arg)
        {
            //TODO:  Replace Naive Impl with something real!!!!
            return new CreateValidateEmailTokenServiceResult(arg.User.FirstName);
        }
    }
}
