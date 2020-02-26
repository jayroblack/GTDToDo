using System.Threading.Tasks;
using HandlebarsDotNet;
using ScooterBear.GTD.Application.Services.MailMerge;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.MailMerge.NewUserEmail
{
    public class
        SendNewUserEmailMergeService : IService<SendNewUserEmailMergeServiceArg, SendNewUserEmailMergeServiceResult>
    {
        public async Task<SendNewUserEmailMergeServiceResult> Run(SendNewUserEmailMergeServiceArg arg)
        {
            var user = arg.User;

            var subjectTemplate = @"Thank you for joining GTD To Do";

            var bodytemplateText = @"
                Thank you {{FirstName}} for signing up for GTD.";

            var bodyTemplateHtml = @"
                Thank you {{FirstName}} for signing up for GTD.";

            var subjectCompiled = Handlebars.Compile(subjectTemplate);
            var bodyTextCompiled = Handlebars.Compile(bodytemplateText);
            var bodyHtmlCompiled = Handlebars.Compile(bodyTemplateHtml);

            var data = new  { 
                user.FirstName,
                user.LastName,
                user.Email,
                user.VersionNumber
            };

            var subjectResult = subjectCompiled(data);
            var textResult = bodyTextCompiled(data);
            var htmlResult = bodyHtmlCompiled(data);

            return new SendNewUserEmailMergeServiceResult(subjectResult, textResult, htmlResult, data);
        }
    }
}
