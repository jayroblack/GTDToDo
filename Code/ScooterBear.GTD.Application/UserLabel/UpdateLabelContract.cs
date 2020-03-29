using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class UpdateLabelNameResult : IServiceResult
    {
        public ILabel Label { get; }

        public UpdateLabelNameResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public class UpdateLabelNameArg : IServiceArgs<UpdateLabelNameResult>
    {
        public string Id { get; }
        public string Name { get; }
        public int VersionNumber { get; }

        public UpdateLabelNameArg(string id, string name, int versionNumber)
        {
            Id = id;
            Name = name;
            VersionNumber = versionNumber;
        }
    }
}
