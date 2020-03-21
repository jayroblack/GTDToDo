using System;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistUpdateProjectResult : IServiceResult
    {
        public PersistUpdateProjectResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IProject Project { get; }
    }

    public class PersistUpdateProjectArg : IServiceArgs<PersistUpdateProjectResult>
    {
        public PersistUpdateProjectArg(Project project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public Project Project { get; }
    }

    public enum PersistUpdateProjectOutcome
    {
        Conflict
    }
}