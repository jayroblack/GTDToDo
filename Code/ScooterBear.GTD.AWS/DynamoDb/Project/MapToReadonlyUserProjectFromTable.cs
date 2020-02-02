using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Project
{
    public class MapToReadonlyUserProjectFromTable : 
        IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject>
    {
        public ReadOnlyProject MapTo(UserProjectLabelDynamoDbTable input)
        {
            return new ReadOnlyProject(input.ID, input.UserId, input.Name, input.Count, input.IsDeleted, input.CountOverDue, input.VersionNumber, input.DateCreated);
        }
    }
}
