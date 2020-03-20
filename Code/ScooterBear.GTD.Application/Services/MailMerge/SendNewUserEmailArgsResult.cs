using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.MailMerge
{
    public class SendNewUserEmailResult : IServiceResult
    {
        public SendNewUserEmailResult(string subject, string text, string html, object data)
        {
            Subject = subject;
            Text = text;
            Html = html;
            Data = data;
        }

        public string Subject { get; }
        public string Text { get; }
        public string Html { get; }
        public object Data { get; }
    }

    public class SendNewUserEmailArg : IServiceArgs<SendNewUserEmailResult>
    {
        public SendNewUserEmailArg(IUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public IUser User { get; }
    }
}