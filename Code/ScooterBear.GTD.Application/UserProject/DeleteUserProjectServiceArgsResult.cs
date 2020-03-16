using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class DeleteUserProjectServiceResult : IServiceResult
    {

    }

    public class DeleteUserProjectServiceArgs : IServiceArgs<DeleteUserProjectServiceResult>
    {
        public string ProjectId { get; }

        public DeleteUserProjectServiceArgs(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
