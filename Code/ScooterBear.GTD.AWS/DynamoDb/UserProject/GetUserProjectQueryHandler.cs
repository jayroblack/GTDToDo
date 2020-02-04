﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Optional;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.AWS.DynamoDb.Project;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.UserProject
{
    public class GetUserProjectQueryHandler : IQueryHandler<GetUserProjectQuery, GetUserProjectQueryResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public GetUserProjectQueryHandler(IDynamoDBFactory dynamoDbFactory, IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetUserProjectQueryResult>> Run(GetUserProjectQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (var dynamoDb = _dynamoDbFactory.Create())
            {
                //Intention is to query the GSI IndexProjectLabelByUserId which projects labels and projects based on the UserId attribute.
                var search = dynamoDb.QueryAsync<UserProjectLabelDynamoDbTable>(query.UserId, new DynamoDBOperationConfig()
                {
                    IndexName = UserProjectLabelTableData.IndexProjectLabelByUserId,
                    QueryFilter = new List<ScanCondition>{ new ScanCondition("Data", ScanOperator.Equal, UserProjectLabelTableData.Project)}
                });

                List<UserProjectLabelDynamoDbTable> results = new List<UserProjectLabelDynamoDbTable>();

                while (!search.IsDone)
                {
                    var getResultBatch = await search.GetNextSetAsync();
                    results.AddRange(getResultBatch);
                }

                if (results.Count == 0)
                    return Option.None<GetUserProjectQueryResult>();

                var resultsMapped = results.Select(x => _mapTo.MapTo(x)).ToList();

                return Option.Some(new GetUserProjectQueryResult(new UserProjects(query.UserId, resultsMapped)));
            }
        }
    }
}
