using System;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistUpdatedUserServiceResult : IServiceResult
    {
        public PersistUpdatedUserServiceResult(IUser updatedUser)
        {
            UpdatedUser = updatedUser ?? throw new ArgumentNullException(nameof(updatedUser));
        }

        public IUser UpdatedUser { get; }
    }

    public class PersistUpdatedUserServiceArg : IServiceArgs<PersistUpdatedUserServiceResult>
    {
        public PersistUpdatedUserServiceArg(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public User User { get; }
    }

    public enum PersistUpdatedUserOutcome
    {
        Conflict
    }
}