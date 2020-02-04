using System;
using ScooterBear.GTD.Application.Projects;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewProjectServiceResult : IServiceResult
    {
        public IProject Project { get; }

        public PersistNewProjectServiceResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class PersistNewProjectServiceArg : IServiceArgs<PersistNewProjectServiceResult>
    {
        public string Id { get; }
        public string UserId { get; }
        public string ProjectName { get; }
        public DateTime DateTimeCreated { get; }

        public PersistNewProjectServiceArg(string id, string userId, string projectName, DateTime dateTimeCreated)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentException($"{nameof(projectName)} is required.");

            if( dateTimeCreated == default(DateTime))
                throw new ArgumentException($"{nameof(dateTimeCreated)} is required.");

            Id = id;
            UserId = userId;
            ProjectName = projectName;
            DateTimeCreated = dateTimeCreated;
        }
    }
}
