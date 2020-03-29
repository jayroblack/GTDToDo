using System;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewLabelResult : IServiceResult
    {
        public ILabel Label { get; }

        public PersistNewLabelResult(ILabel label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }

    public class PersistLabelArg : IServiceArgs<PersistNewLabelResult>
    {
        public string Id { get; }
        public string UserId { get; }
        public string Name { get; }
        public DateTime DateTimeCreated { get; }
        public bool ConsistentRead { get; }

        public PersistLabelArg(string id, string userId, string name, DateTime dateTimeCreated,
            bool consistentRead = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} is required.");

            if (dateTimeCreated == default)
                throw new ArgumentException($"{nameof(dateTimeCreated)} is required.");

            Id = id;
            UserId = userId;
            Name = name;
            DateTimeCreated = dateTimeCreated;
            ConsistentRead = consistentRead;
        }
    }
}
