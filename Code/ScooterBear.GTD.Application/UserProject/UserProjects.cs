using System;
using System.Collections.Generic;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UserProjects
    {
        public string UserId { get; }
        public IEnumerable<IProject> Projects { get; }

        public UserProjects(string userId, IEnumerable<IProject> projects)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");
            UserId = userId;
            Projects = projects ?? throw new ArgumentNullException(nameof(projects));
        }
    }
}
