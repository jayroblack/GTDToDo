using ScooterBear.GTD.Abstractions.Users;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class MapToReadonlyUserFromTable : IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser>
    {
        public ReadonlyUser MapTo(UserProjectLabelDynamoDbTable input)
        {
            return new ReadonlyUser(input.ID, input.FirstName, input.LastName, input.Email, input.IsEmailVerified,
                input.BillingId, input.AuthId, input.IsAccountEnabled, input.VersionNumber.GetValueOrDefault());
        }
    }
}
