using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class GetProjectResult : IQueryResult
    {
        public IProject Project { get; }

        public GetProjectResult(IProject project)
        {
            Project = project;
        }
    }

    public class GetProject : IQuery<GetProjectResult>
    {
        public GetProject(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException(nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}