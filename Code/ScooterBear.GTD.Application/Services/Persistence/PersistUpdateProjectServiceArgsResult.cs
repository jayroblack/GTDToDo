using System;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistUpdateProjectServiceResult : IServiceResult
    {
        public IProject Project { get; }

        public PersistUpdateProjectServiceResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class PersistUpdateProjectServiceArgs : IServiceArgs<PersistUpdateProjectServiceResult>
    {
        public Project Project { get; }

        public PersistUpdateProjectServiceArgs(Project project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public enum PersistUpdateProjectOutcome
    {
        Conflict
    }
}
