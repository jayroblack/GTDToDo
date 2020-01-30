using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.MailMerge
{
    public class SendNewUserSubjectQueryResult : IQueryResult
    {
        public string Text { get; }

        public SendNewUserSubjectQueryResult(string text)
        {
            if( string.IsNullOrEmpty(text))
                throw new ArgumentException($"{nameof(text)} is required.");
            Text = text;
        }
    }

    public class SendNewUserSubjectQueryArgs : IQuery<SendNewUserSubjectQueryResult>
    {
        public IUser User { get; }

        public SendNewUserSubjectQueryArgs(IUser user)
        {
            User = user;
        }
    }
}
