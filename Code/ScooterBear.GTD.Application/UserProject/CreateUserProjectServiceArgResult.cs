using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class CreateNewUserProjectServiceResult : IServiceResult
    {
        public IProject Project { get; }

        public CreateNewUserProjectServiceResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class CreateNewUserProjectServiceArg : IServiceArgs<CreateNewUserProjectServiceResult>
    {
        public string Id { get; }
        public string NewProjectName { get; }
        public bool ConsistentRead { get; }

        public CreateNewUserProjectServiceArg(string id, string newProjectName, bool consistentRead = false)
        {
            Id = id;
            NewProjectName = newProjectName;
            ConsistentRead = consistentRead;
        }
    }
}
