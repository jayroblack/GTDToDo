using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class ProjectQueryResult : IQueryResult
    {
        public IProject UserProject { get; }

        public ProjectQueryResult(IProject userProject)
        {
            UserProject = userProject ?? throw new ArgumentNullException(nameof(userProject));
        }
    }

    public class ProjectQuery : IQuery<ProjectQueryResult>
    {
        public string Id { get; }
        
        public ProjectQuery(string id)
        {
            if( string.IsNullOrEmpty(id))
                throw new ArgumentException(nameof(id));
            Id = id;
        }
    }
}
