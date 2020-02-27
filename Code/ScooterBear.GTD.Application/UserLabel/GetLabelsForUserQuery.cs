using System;
using ScooterBear.GTD.Application.Label;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class GetLabelsForUserQueryResult : IQueryResult
    {
        public UserLabels UserLabels { get; }

        public GetLabelsForUserQueryResult(UserLabels userLabels)
        {
            UserLabels = userLabels ?? throw new ArgumentNullException(nameof(userLabels));
        }
    }

    public class GetLabelsForUserQuery : IQuery<GetLabelsForUserQueryResult>
    {
        public string UserId { get; }

        public GetLabelsForUserQuery(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}
