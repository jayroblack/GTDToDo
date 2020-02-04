using System;
using System.Collections.Generic;
using ScooterBear.GTD.Application.Projects;

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
