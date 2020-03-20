using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class CreateNewProjectResult : IServiceResult
    {
        public CreateNewProjectResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IProject Project { get; }
    }

    public class CreateNewProjectArg : IServiceArgs<CreateNewProjectResult>
    {
        public CreateNewProjectArg(string id, string newProjectName, bool consistentRead = false)
        {
            Id = id;
            NewProjectName = newProjectName;
            ConsistentRead = consistentRead;
        }

        public string Id { get; }
        public string NewProjectName { get; }
        public bool ConsistentRead { get; }
    }
}