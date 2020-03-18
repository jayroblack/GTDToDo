using System;

namespace ScooterBear.GTD.Application.UserProject
{
    public class Project : IProject
    {
        public string Id { get; }
        public string Name { get; private set; }
        public string UserId { get; }
        public int Count { get; private set; }
        public bool IsDeleted { get; private set; }
        public int CountOverDue { get; private set; }
        public int VersionNumber { get; private set; }
        public DateTime DateCreated { get; }

        public Project(string id, string name, string userId, int count, int countOverDue, DateTime dateCreated,
            int versionNumber = 0, bool isDeleted = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id for Project cannot be null", nameof(id));
            Id = id;

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId for Project cannot be null", nameof(userId));
            UserId = userId;

            this.SetName(name);
            this.SetCount(count);
            this.SetCountOverDue(countOverDue);
            this.SetVersionNumber(versionNumber);

            if (dateCreated == DateTime.MaxValue || dateCreated == DateTime.MinValue)
                throw new ArgumentException("DateCreated for Project must be valid.", nameof(dateCreated));

            DateCreated = dateCreated;
            IsDeleted = isDeleted;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));
            
            this.Name = name;
        }

        public void SetCount(int count)
        {
            if( count < 0 )
                throw new ArgumentOutOfRangeException(nameof(count));

            this.Count = count;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            this.IsDeleted = isDeleted;
        }

        public void SetCountOverDue(int countOverDue)
        {
            if (countOverDue < 0)
                throw new ArgumentOutOfRangeException(nameof(countOverDue));

            this.CountOverDue = countOverDue;
        }

        public void SetVersionNumber(int versionNumber)
        {
            if( versionNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(versionNumber));

            this.VersionNumber = versionNumber;
        }
    }
}
