using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class UpdateUserProjectService : IService<UpdateUserProjectServiceArg, UpdateUserProjectServiceResult>
    {
        private readonly IQueryHandler<ProjectQuery, ProjectQueryResult> _getProject;

        public UpdateUserProjectService(IQueryHandler<ProjectQuery, ProjectQueryResult> getProject)
        {
            _getProject = getProject ?? throw new ArgumentNullException(nameof(getProject));
        }

        public Task<UpdateUserProjectServiceResult> Run(UpdateUserProjectServiceArg arg)
        {
            throw new NotImplementedException();
        }
    }
}
