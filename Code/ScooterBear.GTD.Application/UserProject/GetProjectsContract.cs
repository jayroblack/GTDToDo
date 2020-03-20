using System;
using System.Collections.Generic;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class GetProjectsResult : IQueryResult
    {
        public GetProjectsResult(string userId, IEnumerable<IProject> projects)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");
            UserId = userId;
            Projects = projects ?? throw new ArgumentNullException(nameof(projects));
        }

        public string UserId { get; }
        public IEnumerable<IProject> Projects { get; }
    }

    public class GetProjects : IQuery<GetProjectsResult>
    {
        public GetProjects(string userId, bool getOnlyDeleted = false)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            UserId = userId;
            GetOnlyDeleted = getOnlyDeleted;
        }

        public string UserId { get; }
        public bool GetOnlyDeleted { get; }
    }
}