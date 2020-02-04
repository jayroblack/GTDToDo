using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Security
{
    public class CreateValidateEmailTokenServiceResult : IServiceResult
    {
        public string Iv { get; }
        public string Encrypted { get; }

        public CreateValidateEmailTokenServiceResult(string iv, string encrypted)
        {
            if( string.IsNullOrEmpty(iv))
                throw new ArgumentException($"{nameof(iv)} is required.");
            if (string.IsNullOrEmpty(encrypted))
                throw new ArgumentException($"{nameof(encrypted)} is required.");
            Iv = iv;
            Encrypted = encrypted;
        }
    }

    public class CreateValidateEmailTokenServiceArgs : IServiceArgs<CreateValidateEmailTokenServiceResult>, IServiceArgs<ValidateEmailTokenServiceResult>
    {
        public int Version { get; }
        public string UserId { get; }

        public CreateValidateEmailTokenServiceArgs(int version, string userId)
        {
            if( version < 0 )
                throw new ArgumentOutOfRangeException(nameof(version));
            if( string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            Version = version;
            UserId = userId;
        }
    }

    public class CreateValidateEmailTokenServiceArgsResult : IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult>
    {
        private readonly IEncryptDecryptStrategy _encryptDecryptStrategy;
        private readonly IKnowTheDate _iKnowTheDate;

        public CreateValidateEmailTokenServiceArgsResult(IEncryptDecryptStrategy encryptDecryptStrategy,
            IKnowTheDate iKnowTheDate)
        {
            _encryptDecryptStrategy = encryptDecryptStrategy ?? throw new ArgumentNullException(nameof(encryptDecryptStrategy));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
        }

        public async Task<CreateValidateEmailTokenServiceResult> Run(CreateValidateEmailTokenServiceArgs arg)
        {
            var emailUserToken = new EmailUserToken()
            {
                Created = _iKnowTheDate.UtcNow(),
                Version = arg.Version,
                UserId = arg.UserId,
            };

            string json = JsonConvert.SerializeObject(emailUserToken);
            (string iv, string encrypted) = _encryptDecryptStrategy.Encrypt(json);
            return new CreateValidateEmailTokenServiceResult(iv, encrypted);
        }
    }
}
