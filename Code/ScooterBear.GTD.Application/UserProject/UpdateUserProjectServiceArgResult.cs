using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UpdateUserProjectServiceArg : IServiceArgs<UpdateUserProjectServiceResult>
    {
        public string ProjectId { get; }
        public string Name { get; }
        public int Count { get; }
        public bool IsDeleted { get; }
        public int CountOverdue { get; }

        public UpdateUserProjectServiceArg(string projectId, string name, int count, bool isDeleted, int countOverdue)
        {
            ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Count = count;
            IsDeleted = isDeleted;
            CountOverdue = countOverdue;
        }
    }

    public class UpdateUserProjectServiceResult : IServiceResult
    {
        public IProject Project { get; }

        public UpdateUserProjectServiceResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }
}
