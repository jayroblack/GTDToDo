using System;

namespace ScooterBear.GTD.Application.UserLabel
{
    public interface ILabel
    {
        string Id { get; }
        string Name { get; }
        string UserId { get; }
        int Count { get; }
        bool IsDeleted { get; }
        int CountOverDue { get; }
        int? VersionNumber { get; }
        DateTime DateCreated { get; }
    }

    public class Label
    {
        public Label(string id, string name, string userId, int count, bool isDeleted, int countOverDue,
            int? versionNumber, DateTime dateCreated)
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

        public string Id { get; }
        public string Name { get; }
        public string UserId { get; }
        public int Count { get; private set; }
        public bool IsDeleted { get; private set; }
        public int CountOverDue { get; private set; }
        public int? VersionNumber { get; }
        public DateTime DateCreated { get; }

        public void SetCount(int count)
        {
            Count = count;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public void SetCountOverDue(int countOverDue)
        {
            CountOverDue = countOverDue;
        }
    }
}