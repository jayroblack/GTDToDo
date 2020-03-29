using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Labels
{
    public class PersistNewLabelService : IService<PersistLabelArg, PersistNewLabelResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> _mapTo;

        public PersistNewLabelService(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<PersistNewLabelResult> Run(PersistLabelArg arg)
        {
            var table = new UserProjectLabelDynamoDbTable
            {
                ID = arg.Id,
                Data = UserProjectLabelTableData.Label,
                Count = 0,
                CountOverDue = 0,
                Name = arg.Name,
                UserId = arg.UserId,
                DateCreated = arg.DateTimeCreated,
                IsDeleted = false
            };

            using (var dynamoDb = _dynamoDbFactory.Create())
            {
                await dynamoDb.SaveAsync(table, arg.ConsistentRead);

                var labelRetrieved =
                    await dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.Label,
                        CancellationToken.None);

                var readonlyLabel = _mapTo.MapTo(labelRetrieved);

                return new PersistNewLabelResult(readonlyLabel);
            }
        }
    }
}
