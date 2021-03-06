﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class PersistUpdatedUserService : IServiceOpt<PersistUpdatedUserServiceArg,
        PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, User> _mapper;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public PersistUpdatedUserService(IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, User> mapper,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>> Run(
            PersistUpdatedUserServiceArg arg)
        {
            var table = _mapper.MapFrom(arg.User);
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                try
                {
                    await _dynamoDb.SaveAsync(table);

                    var userRetrieved =
                        await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID,
                            UserProjectLabelTableData.User,
                            CancellationToken.None);

                    var readonlyUser = _mapTo.MapTo(userRetrieved);
                    return Option.Some<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                        new PersistUpdatedUserServiceResult(readonlyUser));
                }
                catch (ConditionalCheckFailedException ex)
                {
                    return Option.None<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                        PersistUpdatedUserOutcome.Conflict);
                }
            }
        }
    }
}