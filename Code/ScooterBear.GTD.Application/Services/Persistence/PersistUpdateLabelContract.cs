using System;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistUpdateLabelResult : IServiceResult
    {
        public ILabel Label { get; }

        public PersistUpdateLabelResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public class PersistUpdateLabelArg : IServiceArgs<PersistUpdateLabelResult>
    {
        public Label Label { get; }

        public PersistUpdateLabelArg(Label label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public enum PersistUpdateLabelOutcome
    {
        Conflict
    }
}
