using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users.New
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
        public ReadonlyUser ReadonlyUser { get; }

        public PersistNewUserServiceResult(ReadonlyUser readonlyUser)
        {
            ReadonlyUser = readonlyUser ?? throw new ArgumentNullException(nameof(readonlyUser));
        }
    }
}
