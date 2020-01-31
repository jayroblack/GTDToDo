using System;
using System.Collections.Generic;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UserProjectsQueryResult : IQueryResult
    {
        public IEnumerable<UserProject> UserProjects { get; }

        public UserProjectsQueryResult(IEnumerable<UserProject> userProjects)
        {
            UserProjects = userProjects ?? throw new ArgumentNullException(nameof(userProjects));
        }
    }

    public class UserProjectsQuery : IQuery<UserProjectsQueryResult>
    {
        public string UserId { get; }

        public UserProjectsQuery(string userId)
        {
            if( string.IsNullOrEmpty(userId))
                throw new ArgumentException(nameof(userId));
            UserId = userId;
        }
    }
}
