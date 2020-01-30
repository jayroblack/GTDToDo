using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
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
        public string Iv { get; }
        public string Encrypted { get; }

        public ValidateEmailTokenServicArgs(string iv, string encrypted)
        {
            if( string.IsNullOrEmpty(iv))
                throw new ArgumentException($"{nameof(iv)} is required.");
            if (string.IsNullOrEmpty(encrypted))
                throw new ArgumentException($"{nameof(encrypted)} is required.");
            Iv = iv;
            Encrypted = encrypted;
        }
    }

    public enum ValidateEmailOutcomes
    {
        InvalidToken,
        TokenExpired
    }

    public class ValidateEmailTokenService :
        IServiceOptOutcomes<ValidateEmailTokenServicArgs, ValidateEmailTokenServiceResult, ValidateEmailOutcomes>
    {
        private readonly IEncryptDecryptStrategy _encryptDecryptStrategy;
        private readonly IValidateEmailDurationStrategy _validateEmailDurationStrategy;

        public ValidateEmailTokenService(IEncryptDecryptStrategy encryptDecryptStrategy,
            IValidateEmailDurationStrategy validateEmailDurationStrategy)
        {
            _encryptDecryptStrategy = encryptDecryptStrategy ?? throw new ArgumentNullException(nameof(encryptDecryptStrategy));
            _validateEmailDurationStrategy = validateEmailDurationStrategy ?? throw new ArgumentNullException(nameof(validateEmailDurationStrategy));
        }

        public async Task<Option<ValidateEmailTokenServiceResult, ValidateEmailOutcomes>> Run(ValidateEmailTokenServicArgs arg)
        {
            string decrypted = null;

            try
            {
                decrypted = _encryptDecryptStrategy.Decrypt(arg.Iv, arg.Encrypted);
            }
            catch
            {
                return Option.None<ValidateEmailTokenServiceResult, ValidateEmailOutcomes>(ValidateEmailOutcomes
                    .InvalidToken);
            }

            var emailUserToken = JsonConvert.DeserializeObject<EmailUserToken>(decrypted);
            var isValid = _validateEmailDurationStrategy.IsValid(emailUserToken.Created);

            if( ! isValid)
                return Option.None<ValidateEmailTokenServiceResult, ValidateEmailOutcomes>(ValidateEmailOutcomes
                    .TokenExpired);

            return Option.Some<ValidateEmailTokenServiceResult, ValidateEmailOutcomes>(
                new ValidateEmailTokenServiceResult(true));
        }
    }
}
