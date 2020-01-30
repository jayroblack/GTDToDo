using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        private readonly IEncryptDecryptStrategy _encryptDecryptStrategy;
        
        public CreateValidateEmailTokenService(IEncryptDecryptStrategy encryptDecryptStrategy)
        {
            _encryptDecryptStrategy = encryptDecryptStrategy ?? throw new ArgumentNullException(nameof(encryptDecryptStrategy));
        }

        public async Task<CreateValidateEmailTokenServiceResult> Run(CreateValidateEmailTokenServiceArgs arg)
        {
            var user = arg.User;
            
            var emailUserToken = new EmailUserToken()
            {
                Created = DateTime.UtcNow,
                Version = user.VersionNumber,
                UserId = user.ID,
            };

            string json = JsonConvert.SerializeObject(emailUserToken);
            (string iv, string encrypted) = _encryptDecryptStrategy.Encrypt(json);
            return new CreateValidateEmailTokenServiceResult(string.Concat("?a=", iv, "&b=", encrypted));
        }
    }
}
