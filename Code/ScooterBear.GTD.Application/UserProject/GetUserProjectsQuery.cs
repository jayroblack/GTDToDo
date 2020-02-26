using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class GetUserProjectsQueryResult : IQueryResult
    {
        public UserProjects UserProjects { get; }

        public GetUserProjectsQueryResult(UserProjects userProjects)
        {
            UserProjects = userProjects ?? throw new ArgumentNullException(nameof(userProjects));
        }
    }

    public class GetUserProjectsQuery : IQuery<GetUserProjectsQueryResult>
    {
        public string UserId { get; }

        public GetUserProjectsQuery(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            UserId = userId;
        }
    }
}
