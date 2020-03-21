using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class DeleteProjectResult : IServiceResult
    {
    }

    public class DeleteProjectArg : IServiceArgs<DeleteProjectResult>
    {
        public DeleteProjectArg(string projectId)
        {
            ProjectId = projectId;
        }

        public string ProjectId { get; }
    }
}