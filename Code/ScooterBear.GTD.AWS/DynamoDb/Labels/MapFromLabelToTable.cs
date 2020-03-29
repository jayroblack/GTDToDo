using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Labels
{
    public class MapFromLabelToTable : IMapFrom<UserProjectLabelDynamoDbTable, Label>
    {
        public UserProjectLabelDynamoDbTable MapFrom(Label input)
        {
            var table = new UserProjectLabelDynamoDbTable
            {
                ID = input.Id,
                Name = input.Name,
                UserId = input.UserId,
                Count = input.Count,
                Data = UserProjectLabelTableData.Label,
                DateCreated = input.DateCreated,
                IsDeleted = input.IsDeleted,
                CountOverDue = input.CountOverDue,
                VersionNumber = input.VersionNumber
            };

            return table;
        }
    }
}