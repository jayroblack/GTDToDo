using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class GetLabelResult : IQueryResult
    {
        public GetLabelResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }

        public ILabel Label { get; }
    }

    public class GetLabel : IQuery<GetLabelResult>
    {
        public GetLabel(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException(nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}