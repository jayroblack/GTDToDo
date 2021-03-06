﻿using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class MapFromUserToTable : IMapFrom<UserProjectLabelDynamoDbTable, User>
    {
        public UserProjectLabelDynamoDbTable MapFrom(User user)
        {
            var table = new UserProjectLabelDynamoDbTable
            {
                ID = user.ID,
                Data = UserProjectLabelTableData.User,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAccountEnabled = user.IsAccountEnabled.GetValueOrDefault(),
                BillingId = user.BillingId,
                AuthId = user.AuthId,
                VersionNumber = user.VersionNumber,
                DateCreated = user.DateCreated
            };
            return table;
        }
    }
}