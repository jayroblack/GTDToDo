using System;

namespace ScooterBear.GTD.Application.Projects
{
    public class Project : IProject
    {
        public string Id { get; }
        public string Name { get; }
        public string UserId { get; }
        public int Count { get; private set; }
        public bool IsDeleted { get; private set; }
        public int CountOverDue { get; private set; }
        public int? VersionNumber { get; }
        public DateTime DateCreated { get; }

        public Project(string id, string name, string userId, int count, bool isDeleted, int countOverDue, int? versionNumber, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            UserId = userId;
            Count = count;
            IsDeleted = isDeleted;
            CountOverDue = countOverDue;
            VersionNumber = versionNumber;
            DateCreated = dateCreated;
        }

        public void SetCount(int count)
        {
            this.Count = count;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            this.IsDeleted = isDeleted;
        }

        public void SetCountOverDue(int countOverDue)
        {
            this.CountOverDue = countOverDue;
        }
    }
}
