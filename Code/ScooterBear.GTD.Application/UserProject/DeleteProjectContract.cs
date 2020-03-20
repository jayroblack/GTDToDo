using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class DeleteProjectResult : IServiceResult
    {
    }

    public class DeleteProjectArgs : IServiceArgs<DeleteProjectResult>
    {
        public DeleteProjectArgs(string projectId)
        {
            ProjectId = projectId;
        }

        public string ProjectId { get; }
    }
}