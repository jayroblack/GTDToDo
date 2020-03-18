using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Projects
{
    public class MapToReadonlyProjectFromTable :
        IMapTo<UserProjectLabelDynamoDbTable, ReadOnlyProject>
    {
        public ReadOnlyProject MapTo(UserProjectLabelDynamoDbTable input)
        {
            return new ReadOnlyProject(input.ID, input.UserId, input.Name, input.Count, input.IsDeleted, input.CountOverDue, input.VersionNumber.GetValueOrDefault(), input.DateCreated);
        }
    }
}
