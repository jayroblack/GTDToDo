using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Label
{
    public class LabelQueryResult : IQueryResult
    {
        public ILabel Label { get; }

        public LabelQueryResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public class LabelQuery : IQuery<LabelQueryResult>
    {
        public string Id { get; }

        public LabelQuery(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException(nameof(id));
            Id = id;
        }
    }
}
