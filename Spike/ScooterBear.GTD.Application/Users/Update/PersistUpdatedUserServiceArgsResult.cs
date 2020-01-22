using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class PersistUpdatedUserServiceResult : IServiceResult
    {
        public IUser UpdatedUser { get; }

        public PersistUpdatedUserServiceResult(IUser updatedUser)
        {
            UpdatedUser = updatedUser ?? throw new ArgumentNullException(nameof(updatedUser));
        }
    }

    public class PersistUpdatedUserServiceArgs : IServiceArgs<PersistUpdatedUserServiceResult>
    {
        public User User { get; }

        public PersistUpdatedUserServiceArgs(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public enum PersistUpdatedUserOutcome
    {
        Conflict
    }
}
