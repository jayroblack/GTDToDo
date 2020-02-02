using System;

namespace ScooterBear.GTD.Application.Projects
{
    public interface IProject
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
}
