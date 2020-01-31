using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Application.Services.MailMerge;
using ScooterBear.GTD.Application.Services.Routes;
using ScooterBear.GTD.Application.Services.Security;
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
        private readonly IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult> _createToken;
        private readonly IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult> _sendNewUserMailMerge;
        private readonly IService<RouteServiceArgs, RouteServiceResult> _routeService;
        private readonly ICommandHandler<SendEmailCommand> _sendEmail;
        private readonly IEmailConfigurationFactory _emailConfigurationFactory;

        public SendNewUserEmailService(
            IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult> createToken,
            IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult> sendNewUserMailMerge,
            IService<RouteServiceArgs, RouteServiceResult> routeService,
            ICommandHandler<SendEmailCommand> sendEmail,
            IEmailConfigurationFactory emailConfigurationFactory)
        {
            _createToken = createToken ?? throw new ArgumentNullException(nameof(createToken));
            _sendNewUserMailMerge = sendNewUserMailMerge ?? throw new ArgumentNullException(nameof(sendNewUserMailMerge));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _sendEmail = sendEmail ?? throw new ArgumentNullException(nameof(sendEmail));
            _emailConfigurationFactory = emailConfigurationFactory ?? throw new ArgumentNullException(nameof(emailConfigurationFactory));
        }
        public async Task<SendNewUserEmailServiceResult> Run(SendNewUserEmailServiceArgs arg)
        {
            var token = await _createToken.Run(new CreateValidateEmailTokenServiceArgs(arg.User.VersionNumber, arg.User.ID));

            var route = await _routeService.Run(new RouteServiceArgs(RouteName.ValidateEmail));

            var mailMergeResult =
                _sendNewUserMailMerge.Run(new SendNewUserEmailMergeServiceArg(arg.User, token.Iv, token.Encrypted, route.Url));

            var config = _emailConfigurationFactory.Create();

            var result = mailMergeResult.Result;

            await _sendEmail.Run(new SendEmailCommand(config.FromEmailAddress, arg.User.Email, result.Subject, result.Text,
                result.Html, config.ConfigSetName, result.Data, EmailType.VerifyEmail));
            return new SendNewUserEmailServiceResult();
        }
    }
}
