using System;
using System.Threading.Tasks;
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

        public SendNewUserEmailService(
            IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult> createToken,
            IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult> sendNewUserMailMerge,
            IService<RouteServiceArgs, RouteServiceResult> routeService)
        {
            _createToken = createToken ?? throw new ArgumentNullException(nameof(createToken));
            _sendNewUserMailMerge = sendNewUserMailMerge ?? throw new ArgumentNullException(nameof(sendNewUserMailMerge));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
        }
        public async Task<SendNewUserEmailServiceResult> Run(SendNewUserEmailServiceArgs arg)
        {
            var token = await _createToken.Run(new CreateValidateEmailTokenServiceArgs(arg.User.VersionNumber, arg.User.ID));

            var route = await _routeService.Run(new RouteServiceArgs(RouteName.ValidateEmail));

            var mailMergeResult =
                _sendNewUserMailMerge.Run(new SendNewUserEmailMergeServiceArg(arg.User, token.Iv, token.Encrypted, route.Url));

            
            //TODO:  2) Email Send SubSystem - Send Emails.
            //TODO:  3) Test Fake - Persist Email Data - verify email was sent when new user was created.
            //TODO:  4) Create a REST API Method to receive that verification and mark the user as Email Verified.
            throw new NotImplementedException();
        }
    }
}
