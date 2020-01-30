using System;
using System.Threading.Tasks;
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

        public SendNewUserEmailService(
            IService<CreateValidateEmailTokenServiceArgs, CreateValidateEmailTokenServiceResult> createToken)
        {
            _createToken = createToken ?? throw new ArgumentNullException(nameof(createToken));
        }
        public Task<SendNewUserEmailServiceResult> Run(SendNewUserEmailServiceArgs arg)
        {
            
            //TODO:  1) Create Cryptographic Method To Create a Time Sensitive Token to attach to a URL to verify this Email.
            //TODO:  2) Create a REST API Method to receive that verification and mark the user as Email Verified.
            //TODO:  3) Email Merge SubSystem - Maybe HandlerBars - Send Data and a Template and get an output of info. 
            //TODO:  4) Email Send SubSystem - Send Emails.
            //TODO:  5) Test Fake - Persist Email Data - verify email was sent when new user was created.
            throw new NotImplementedException();
        }
    }
}
