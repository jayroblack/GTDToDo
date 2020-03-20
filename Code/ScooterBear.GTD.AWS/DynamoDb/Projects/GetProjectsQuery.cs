using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Optional;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class GetProjectsQuery : IQueryHandler<GetProjects, GetProjectsResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public GetProjectsQuery(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetProjectsResult>> Run(GetProjects query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (var dynamoDb = _dynamoDbFactory.Create())
            {
                var search = dynamoDb.QueryAsync<UserProjectLabelDynamoDbTable>(query.UserId,
                    new DynamoDBOperationConfig
                    {
                        IndexName = UserProjectLabelTableData.IndexProjectLabelByUserId,
                        QueryFilter = new List<ScanCondition>
                        {
                            new ScanCondition("Data", ScanOperator.Equal, UserProjectLabelTableData.Project),
                            new ScanCondition("IsDeleted", ScanOperator.Equal, query.GetOnlyDeleted)
                        }
                    });

                var results = new List<UserProjectLabelDynamoDbTable>();

                while (!search.IsDone)
                {
                    var getResultBatch = await search.GetNextSetAsync();
                    results.AddRange(getResultBatch);
                }

                if (results.Count == 0)
                    return Option.None<GetProjectsResult>();

                var resultsMapped = results.Select(x => _mapTo.MapTo(x)).ToList();

                return Option.Some(new GetProjectsResult(query.UserId, resultsMapped));
            }
        }
    }
}