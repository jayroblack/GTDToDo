using System;
using ScooterBear.GTD.Application.Projects;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class CreateNewUserProjectServiceResult : IServiceResult
    {
        public IProject Project { get; }

        public CreateNewUserProjectServiceResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class CreateNewUserProjectServiceArg : IServiceArgs<CreateNewUserProjectServiceResult>
    {
        public string Id { get; }
        public string UserId { get; }
        public string NewProjectName { get; }

        public CreateNewUserProjectServiceArg(string id, string userId, string newProjectName)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if ( string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            if (string.IsNullOrEmpty(newProjectName))
                throw new ArgumentException($"{nameof(newProjectName)} is required.");

            Id = id;
            UserId = userId;
            NewProjectName = newProjectName;
        }
    }
}
