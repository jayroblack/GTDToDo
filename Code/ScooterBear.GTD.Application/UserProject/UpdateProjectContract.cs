using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UpdateProjectNameResult : IServiceResult
    {
        public UpdateProjectNameResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IProject Project { get; }
    }

    public class UpdateProjectNameArg : IServiceArgs<UpdateProjectNameResult>
    {
        public UpdateProjectNameArg(string id, string name, int versionNumber)
        {
            Id = id;
            Name = name;
            VersionNumber = versionNumber;
        }

        public string Id { get; }
        public string Name { get; }
        public int VersionNumber { get; }
    }
}