using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Label
{
    public class GetLabelQuery : IQueryHandler<GetLabel, GetLabelResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> _mapTo;

        public GetLabelQuery(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetLabelResult>> Run(GetLabel query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                var result =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(query.Id,
                        UserProjectLabelTableData.Label);

                if (result == null)
                    return Option.None<GetLabelResult>();

                var mapped = _mapTo.MapTo(result);
                return Option.Some(new GetLabelResult(mapped));
            }
        }
    }
}