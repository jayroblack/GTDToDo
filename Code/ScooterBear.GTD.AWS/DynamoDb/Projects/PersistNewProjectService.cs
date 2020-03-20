using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class PersistNewProjectService : IService<PersistNewProjectArg, PersistNewProjectResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public PersistNewProjectService(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<PersistNewProjectResult> Run(PersistNewProjectArg arg)
        {
            var table = new UserProjectLabelDynamoDbTable
            {
                ID = arg.Id,
                Data = UserProjectLabelTableData.Project,
                Count = 0,
                CountOverDue = 0,
                Name = arg.ProjectName,
                UserId = arg.UserId,
                DateCreated = arg.DateTimeCreated,
                IsDeleted = false
            };

            using (var dynamoDb = _dynamoDbFactory.Create())
            {
                await dynamoDb.SaveAsync(table, arg.ConsistentRead);

                var projectRetrieved =
                    await dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.Project,
                        CancellationToken.None);

                var readonlyProject = _mapTo.MapTo(projectRetrieved);

                return new PersistNewProjectResult(readonlyProject);
            }
        }
    }
}