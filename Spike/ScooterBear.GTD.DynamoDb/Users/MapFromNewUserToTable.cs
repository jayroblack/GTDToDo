using ScooterBear.GTD.Abstractions.Users.New;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class MapFromNewUserToTable : IMapFrom<UserProjectLabelDynamoDbTable, NewUser>
    {
        public UserProjectLabelDynamoDbTable MapFrom(NewUser newUser)
        {
            var table = new UserProjectLabelDynamoDbTable()
            {
                ID = newUser.ID,
                Data = UserProjectLabelTableData.User,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                IsEmailVerified = newUser.IsEmailVerified,
                IsAccountEnabled = newUser.IsAccountEnabled,
                DateCreated = newUser.DateCreated
            };
            return table;
        }
    }
}
