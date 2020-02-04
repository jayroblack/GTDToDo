using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Label;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Label
{
    public class LabelQueryHandler : IQueryHandler<LabelQuery, LabelQueryResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> _mapTo;

        public LabelQueryHandler(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<LabelQueryResult>> Run(LabelQuery query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                var result =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(query.Id,
                        UserProjectLabelTableData.Label);

                if (result == null)
                    return Option.None<LabelQueryResult>();

                var mapped = _mapTo.MapTo(result);
                return Option.Some(new LabelQueryResult(mapped));
            }
        }
    }
}
