using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class ProjectQueryHandler : IQueryHandler<ProjectQuery, ProjectQueryResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> _mapTo;

        public ProjectQueryHandler(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<ProjectQueryResult>> Run(ProjectQuery query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                var result =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(query.Id,
                        UserProjectLabelTableData.Project);

                if (result == null)
                    return Option.None<ProjectQueryResult>();

                var mapped = _mapTo.MapTo(result);
                return Option.Some(new ProjectQueryResult(mapped));
            }
        }
    }
}
