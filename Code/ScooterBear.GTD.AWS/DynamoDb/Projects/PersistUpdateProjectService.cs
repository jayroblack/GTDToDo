using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class PersistUpdateProjectService : IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, Project> _map;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public PersistUpdateProjectService(
            IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, Project> map,
            IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>> Run(PersistUpdateProjectServiceArgs arg)
        {
            var table = _map.MapFrom(arg.Project);
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                try
                {
                    await _dynamoDb.SaveAsync(table);

                    UserProjectLabelDynamoDbTable projectRetrieved =
                        await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.Project,
                            CancellationToken.None);

                    var readOnlyProject = _mapTo.MapTo(projectRetrieved);
                    return Option.Some<PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>(
                        new PersistUpdateProjectServiceResult(readOnlyProject));

                }
                catch (ConditionalCheckFailedException ex)
                {
                    return Option.None<PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>(
                        PersistUpdateProjectOutcome.Conflict);
                }
            }
        }
    }
}
