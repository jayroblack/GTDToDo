using System;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistUpdateProjectResult : IServiceResult
    {
        public IProject Project { get; }

        public PersistUpdateProjectResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class PersistUpdateProjectArg : IServiceArgs<PersistUpdateProjectResult>
    {
        public Project Project { get; }

        public PersistUpdateProjectArg(Project project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public enum PersistUpdateProjectOutcome
    {
        Conflict
    }
}