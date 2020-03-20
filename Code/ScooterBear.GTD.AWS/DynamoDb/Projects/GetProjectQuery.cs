using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class GetProjectQuery : IQueryHandler<GetProject, GetProjectResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public GetProjectQuery(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetProjectResult>> Run(GetProject query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                var result =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(query.Id,
                        UserProjectLabelTableData.Project);

                if (result == null)
                    return Option.None<GetProjectResult>();

                var mapped = _mapTo.MapTo(result);
                return Option.Some(new GetProjectResult(mapped));
            }
        }
    }
}