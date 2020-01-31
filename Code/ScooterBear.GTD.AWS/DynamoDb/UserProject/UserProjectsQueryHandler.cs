using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Optional;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.UserProject
{
    public class UserProjectsQueryHandler : IQueryHandler<UserProjectsQuery, UserProjectsQueryResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;

        public UserProjectsQueryHandler(IDynamoDBFactory dynamoDbFactory)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
        }

        public async Task<Option<UserProjectsQueryResult>> Run(UserProjectsQuery query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                //TODO:  WTF!!!
                throw new NotImplementedException();
            }
        }
    }
}
