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

            //Imaging there are many emails we need to send - and that these emails will be moore
            //complicated.  Also imagine a world where the templates can be changed without having to deploy the code.
            var subjectTemplate = @"Thank you for joining GTD To Do, please verify your email.";

            var bodytemplateText = @"
                Thank you {{FirstName}} for signing up for GTD.  
                Please lick the link or paste the link into a browser to verify your email address: {{route}}?secret={{secret}}&key={{key}}";

            var bodyTemplateHtml = @"
                Thank you {{FirstName}} for signing up for GTD.  
                Please lick the link or paste the link into a browser to verify your email address: {{route}}?secret={{secret}}&key={{key}}";

            var subjectCompiled = Handlebars.Compile(subjectTemplate);
            var bodyTextCompiled = Handlebars.Compile(bodytemplateText);
            var bodyHtmlCompiled = Handlebars.Compile(bodyTemplateHtml);

            var data = new  { 
                user.FirstName,
                user.LastName,
                user.Email,
                user.VersionNumber,
                arg.Key,
                arg.Route,
                arg.Secret
            };

            var subjectResult = subjectCompiled(data);
            var textResult = bodyTextCompiled(data);
            var htmlResult = bodyHtmlCompiled(data);

            return new SendNewUserEmailMergeServiceResult(subjectResult, textResult, htmlResult);
        }
    }
}
