using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class CreateNewLabelResult : IServiceResult
    {
        public ILabel Label { get; }

        public CreateNewLabelResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public class CreateNewLabelArg : IServiceArgs<CreateNewLabelResult>
    {
        public string Id { get; }
        public string Name { get; }
        public bool ConsistentRead { get; }

        public CreateNewLabelArg(string id, string name, bool consistentRead = false)
        {
            Id = id;
            Name = name;
            ConsistentRead = consistentRead;
        }
    }
}
