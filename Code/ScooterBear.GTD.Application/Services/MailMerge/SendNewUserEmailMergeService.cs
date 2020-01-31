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

        public SendNewUserEmailMergeServiceResult(string subject, string text, string html)
        {
            Subject = subject;  
            Text = text;
            Html = html;
        }
    }

    public class SendNewUserEmailMergeServiceArg : IServiceArgs<SendNewUserEmailMergeServiceResult>
    {
        public IUser User { get; }
        public string Secret { get; }
        public string Key { get; }
        public string Route { get; }

        public SendNewUserEmailMergeServiceArg(IUser user, string secret, string key, string route)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            if( string.IsNullOrEmpty(secret))
                throw new ArgumentException($"{nameof(secret)} is required.");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"{nameof(key)} is required.");
            if (string.IsNullOrEmpty(route))
                throw new ArgumentException($"{nameof(route)} is required.");
            Secret = secret;
            Key = key;
            Route = route;
        }
    }
}
