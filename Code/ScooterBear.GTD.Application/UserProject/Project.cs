using System;

namespace ScooterBear.GTD.Application.UserProject
{
    public interface IProject
    {
        string Id { get; }
        string Name { get; }
        string UserId { get; }
        int Count { get; }
        bool IsDeleted { get; }
        int CountOverDue { get; }
        int VersionNumber { get; }
        DateTime DateCreated { get; }
    }

    public class Project : IProject
    {
        public Project(string id, string name, string userId, int count, int countOverDue, DateTime dateCreated,
            int versionNumber = 0, bool isDeleted = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id for Project cannot be null", nameof(id));
            Id = id;

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId for Project cannot be null", nameof(userId));
            UserId = userId;

            SetName(name);
            SetCount(count);
            SetCountOverDue(countOverDue);
            SetVersionNumber(versionNumber);
            SetIsDeleted(isDeleted);

            if (dateCreated == DateTime.MaxValue || dateCreated == DateTime.MinValue)
                throw new ArgumentException("DateCreated for Project must be valid.", nameof(dateCreated));

            DateCreated = dateCreated;
        }

        public string Id { get; }
        public string Name { get; private set; }
        public string UserId { get; }
        public int Count { get; private set; }
        public bool IsDeleted { get; private set; }
        public int CountOverDue { get; private set; }
        public int VersionNumber { get; private set; }
        public DateTime DateCreated { get; }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            Name = name;
        }

        public void SetCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public void SetCountOverDue(int countOverDue)
        {
            if (countOverDue < 0)
                throw new ArgumentOutOfRangeException(nameof(countOverDue));

            CountOverDue = countOverDue;
        }

        public void SetVersionNumber(int versionNumber)
        {
            if (versionNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(versionNumber));

            VersionNumber = versionNumber;
        }
    }
}