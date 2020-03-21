using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewUserArg : IServiceArgs<PersistNewUserResult>
    {
        public PersistNewUserArg(NewUser newUser, bool consistentRead = false)
        {
            NewUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            ConsistentRead = consistentRead;
        }

        public NewUser NewUser { get; }
        public bool ConsistentRead { get; }
    }

    public class PersistNewUserResult : IServiceResult
    {
        public PersistNewUserResult(IUser readonlyUser)
        {
            ReadonlyUser = readonlyUser ?? throw new ArgumentNullException(nameof(readonlyUser));
        }

        public IUser ReadonlyUser { get; }
    }
}