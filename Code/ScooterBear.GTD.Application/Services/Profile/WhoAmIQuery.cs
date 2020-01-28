using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Profile
{
    public class WhoAmIQueryResult : IQueryResult
    {
        public IUser CurrentlyLoggedInUser { get; }

        public WhoAmIQueryResult(IUser currentlyLoggedInUser)
        {
            CurrentlyLoggedInUser = currentlyLoggedInUser ?? throw new ArgumentNullException(nameof(currentlyLoggedInUser));
        }
    }

    public class WhoAmIQueryArgs : IQuery<WhoAmIQueryResult>
    {
        public WhoAmIQueryArgs()
        {
            
        }
    }

    public class WhoAmIQueryHandler : IQueryHandler<WhoAmIQueryArgs, WhoAmIQueryResult>
    {
        public Task<Option<WhoAmIQueryResult>> Run(WhoAmIQueryArgs query)
        {
            //Detect from token - session - something who you are.
            //Search Dynamo DB on an Index that searches AuthId and matches to user
            throw new NotImplementedException();
        }
    }
}
