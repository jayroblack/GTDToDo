using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Application.Services.MailMerge;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public class SendNewUserEmailServiceResult : IServiceResult
    {
    }

    public class SendNewUserEmailServiceArgs : IServiceArgs<SendNewUserEmailServiceResult>
    {
        public IUser User { get; }

        public SendNewUserEmailServiceArgs(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class SendNewUserEmailService : IService<SendNewUserEmailServiceArgs, SendNewUserEmailServiceResult>{
        private readonly IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult> _sendNewUserMailMerge;
        private readonly ICommandHandler<SendEmailCommand> _sendEmail;
        private readonly IEmailConfigurationFactory _emailConfigurationFactory;

        public SendNewUserEmailService(
            IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult> sendNewUserMailMerge,
            ICommandHandler<SendEmailCommand> sendEmail,
            IEmailConfigurationFactory emailConfigurationFactory)
        {
            _sendNewUserMailMerge = sendNewUserMailMerge ?? throw new ArgumentNullException(nameof(sendNewUserMailMerge));
            _sendEmail = sendEmail ?? throw new ArgumentNullException(nameof(sendEmail));
            _emailConfigurationFactory = emailConfigurationFactory ?? throw new ArgumentNullException(nameof(emailConfigurationFactory));
        }
        public async Task<SendNewUserEmailServiceResult> Run(SendNewUserEmailServiceArgs arg)
        {
            var mailMergeResult =
                _sendNewUserMailMerge.Run(new SendNewUserEmailMergeServiceArg(arg.User));

            var config = _emailConfigurationFactory.Create();

            var result = mailMergeResult.Result;

            await _sendEmail.Run(new SendEmailCommand(config.FromEmailAddress, arg.User.Email, result.Subject, result.Text,
                result.Html, config.ConfigSetName, result.Data, EmailType.VerifyEmail));
            return new SendNewUserEmailServiceResult();
        }
    }
}
