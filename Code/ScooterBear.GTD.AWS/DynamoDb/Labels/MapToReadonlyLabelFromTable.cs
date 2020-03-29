using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Labels
{
    public class MapToReadonlyLabelFromTable :
        IMapTo<UserProjectLabelDynamoDbTable, ReadonlyLabel>
    {
        public ReadonlyLabel MapTo(UserProjectLabelDynamoDbTable input)
        {
            return new ReadonlyLabel(input.ID, input.UserId, input.Name, input.Count, input.IsDeleted,
                input.CountOverDue, input.VersionNumber.GetValueOrDefault(), input.DateCreated);
        }
    }
}