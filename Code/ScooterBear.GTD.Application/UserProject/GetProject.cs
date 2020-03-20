using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class GetProjectResult : IQueryResult
    {
        public GetProjectResult(IProject userProject)
        {
            UserProject = userProject ?? throw new ArgumentNullException(nameof(userProject));
        }

        public IProject UserProject { get; }
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