using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.MailMerge
{
    public class SendNewUserEmailMergeServiceResult : IServiceResult
    {
        public string Subject { get; }
        public string Text { get; }
        public string Html { get; }
        public object Data { get; }

        public SendNewUserEmailMergeServiceResult(string subject, string text, string html, object data)
        {
            Subject = subject;  
            Text = text;
            Html = html;
            Data = data;
        }
    }

    public class SendNewUserEmailMergeServiceArg : IServiceArgs<SendNewUserEmailMergeServiceResult>
    {
        public IUser User { get; }
        
        public SendNewUserEmailMergeServiceArg(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
