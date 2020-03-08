using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    
    public class MapFromProjectToTable : IMapFrom<UserProjectLabelDynamoDbTable, Project>
    {
        public UserProjectLabelDynamoDbTable MapFrom(Project input)
        {
            var table = new UserProjectLabelDynamoDbTable()
            {
                ID = input.Id,
                Name = input.Name,
                UserId = input.UserId,
                Count = input.Count,
                Data = UserProjectLabelTableData.Project,
                DateCreated = input.DateCreated,
                IsDeleted = input.IsDeleted,
                CountOverDue = input.CountOverDue,
                VersionNumber = input.VersionNumber,
            };

            return table;
        }
    }
}
