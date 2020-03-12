using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewUserServiceArgs : IServiceArgs<PersistNewUserServiceResult>
    {
        public NewUser NewUser { get; }
        public bool ConsistentRead { get; }

        public PersistNewUserServiceArgs(NewUser newUser, bool consistentRead = false)
        {
            NewUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            ConsistentRead = consistentRead;
        }
    }

    public class PersistNewUserServiceResult : IServiceResult
    {
        public IUser ReadonlyUser { get; }

        public PersistNewUserServiceResult(IUser readonlyUser)
        {
            ReadonlyUser = readonlyUser ?? throw new ArgumentNullException(nameof(readonlyUser));
        }
    }
}
