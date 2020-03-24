using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UpdateProjectNameArg : IServiceArgs<UpdateProjectNameResult>
    {
        public UpdateProjectNameArg(string projectId, string name, int versionNumber)
        {
            ProjectId = projectId;
            Name = name;
            VersionNumber = versionNumber;
        }

        public string ProjectId { get; }
        public string Name { get; }
        public int VersionNumber { get; }
    }

    public class UpdateProjectNameResult : IServiceResult
    {
        public UpdateProjectNameResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IProject Project { get; }
    }
}