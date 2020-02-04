using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class GetUserProjectQueryResult : IQueryResult
    {
        public UserProjects UserProjects { get; }

        public GetUserProjectQueryResult(UserProjects userProjects)
        {
            UserProjects = userProjects ?? throw new ArgumentNullException(nameof(userProjects));
        }
    }

    public class GetUserProjectQuery : IQuery<GetUserProjectQueryResult>
    {
        public string UserId { get; }

        public GetUserProjectQuery(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            UserId = userId;
        }
    }
}
