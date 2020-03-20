using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UpdateProjectArg : IServiceArgs<UpdateProjectResult>
    {
        public UpdateProjectArg(string projectId, string name, int count, bool isDeleted, int countOverdue,
            int versionNumber)
        {
            ProjectId = projectId;
            Name = name;
            Count = count;
            IsDeleted = isDeleted;
            CountOverdue = countOverdue;
            VersionNumber = versionNumber;
        }

        public string ProjectId { get; }
        public string Name { get; }
        public int Count { get; }
        public bool IsDeleted { get; }
        public int CountOverdue { get; }
        public int VersionNumber { get; }
    }

    public class UpdateProjectResult : IServiceResult
    {
        public UpdateProjectResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IProject Project { get; }
    }
}