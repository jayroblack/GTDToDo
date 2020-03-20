using System;
using System.Collections.Generic;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class GetLabelsResult : IQueryResult
    {
        public GetLabelsResult(string userId, IEnumerable<ILabel> projects)
        {
            UserId = userId;
            Labels = projects ?? throw new ArgumentNullException(nameof(projects));
        }

        public string UserId { get; }
        public IEnumerable<ILabel> Labels { get; }
    }

    public class GetLabels : IQuery<GetLabelsResult>
    {
        public GetLabels(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        public string UserId { get; }
    }
}