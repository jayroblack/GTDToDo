using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Labels
{
    public class PersistUpdateLabelService : IServiceOpt<PersistUpdateLabelArg, PersistUpdateLabelResult,
        PersistUpdateLabelOutcome>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, Label> _map;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> _mapTo;

        public PersistUpdateLabelService(
            IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, Label> map,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<PersistUpdateLabelResult, PersistUpdateLabelOutcome>> Run(
            PersistUpdateLabelArg arg)
        {
            var table = _map.MapFrom(arg.Label);
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                try
                {
                    await _dynamoDb.SaveAsync(table);

                    var labelRetrieved =
                        await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID,
                            UserProjectLabelTableData.Label,
                            CancellationToken.None);

                    var readOnlyLabel = _mapTo.MapTo(labelRetrieved);
                    return Option.Some<PersistUpdateLabelResult, PersistUpdateLabelOutcome>(
                        new PersistUpdateLabelResult(readOnlyLabel));
                }
                catch (ConditionalCheckFailedException ex)
                {
                    return Option.None<PersistUpdateLabelResult, PersistUpdateLabelOutcome>(
                        PersistUpdateLabelOutcome.Conflict);
                }
            }
        }
    }
}