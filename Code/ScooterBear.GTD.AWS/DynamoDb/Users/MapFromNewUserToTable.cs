using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class MapFromNewUserToTable : IMapFrom<UserProjectLabelDynamoDbTable, NewUser>
    {
        public UserProjectLabelDynamoDbTable MapFrom(NewUser newUser)
        {
            var table = new UserProjectLabelDynamoDbTable
            {
                ID = newUser.ID,
                Data = UserProjectLabelTableData.User,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                IsAccountEnabled = newUser.IsAccountEnabled,
                DateCreated = newUser.DateCreated
            };
            return table;
        }
    }
}