using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewUserServiceArgs : IServiceArgs<PersistNewUserServiceResult>
    {
        public NewUser NewUser { get; }

        public PersistNewUserServiceArgs(NewUser newUser)
        {
            NewUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
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
