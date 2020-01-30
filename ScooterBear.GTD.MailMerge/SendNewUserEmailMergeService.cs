using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.MailMerge;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.MailMerge
{
    public class SendNewUserEmailMergeService : IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult>
    {
        public Task<SendNewUserEmailMergeServiceResult> Run(SendNewUserEmailMergeServiceArg arg)
        {
            throw new NotImplementedException();
        }
    }
}
