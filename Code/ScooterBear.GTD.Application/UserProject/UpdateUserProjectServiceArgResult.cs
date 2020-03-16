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
        public int VersionNumber { get; }

        public UpdateUserProjectServiceArg(string projectId, string name, int count, bool isDeleted, int countOverdue, int versionNumber)
        {
            ProjectId = projectId;
            Name = name;
            Count = count;
            IsDeleted = isDeleted;
            CountOverdue = countOverdue;
            VersionNumber = versionNumber;
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
