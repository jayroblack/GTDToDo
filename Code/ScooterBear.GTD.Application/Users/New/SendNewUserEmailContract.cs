using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.New
{
    public class SendNewUserEmailResult : IServiceResult
    {
    }

    public class SendNewUserEmailArg : IServiceArgs<SendNewUserEmailResult>
    {
        public SendNewUserEmailArg(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public IUser User { get; }
    }

    public class SendNewUserEmailService : IService<SendNewUserEmailArg, SendNewUserEmailResult>
    {
        private readonly IEmailConfigurationFactory _emailConfigurationFactory;
        private readonly ICommandHandler<SendEmailCommand> _sendEmail;

        private readonly IService<Services.MailMerge.SendNewUserEmailArg, Services.MailMerge.SendNewUserEmailResult>
            _sendNewUserMailMerge;

        public SendNewUserEmailService(
            IService<Services.MailMerge.SendNewUserEmailArg, Services.MailMerge.SendNewUserEmailResult>
                sendNewUserMailMerge,
            ICommandHandler<SendEmailCommand> sendEmail,
            IEmailConfigurationFactory emailConfigurationFactory)
        {
            _sendNewUserMailMerge =
                sendNewUserMailMerge ?? throw new ArgumentNullException(nameof(sendNewUserMailMerge));
            _sendEmail = sendEmail ?? throw new ArgumentNullException(nameof(sendEmail));
            _emailConfigurationFactory = emailConfigurationFactory ??
                                         throw new ArgumentNullException(nameof(emailConfigurationFactory));
        }

        public async Task<SendNewUserEmailResult> Run(SendNewUserEmailArg arg)
        {
            var mailMergeResult =
                _sendNewUserMailMerge.Run(new Services.MailMerge.SendNewUserEmailArg(arg.User));

            var config = _emailConfigurationFactory.Create();

            var result = mailMergeResult.Result;

            await _sendEmail.Run(new SendEmailCommand(config.FromEmailAddress, arg.User.Email, result.Subject,
                result.Text,
                result.Html, config.ConfigSetName, result.Data, EmailType.VerifyEmail));
            return new SendNewUserEmailResult();
        }
    }
}