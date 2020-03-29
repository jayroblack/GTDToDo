using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class DeleteLabelResult : IServiceResult
    {

    }

    public class DeleteLabelArg : IServiceArgs<DeleteLabelResult>
    {
        public string LabelId { get; }

        public DeleteLabelArg(string labelId)
        {
            LabelId = labelId;
        }
    }
}
