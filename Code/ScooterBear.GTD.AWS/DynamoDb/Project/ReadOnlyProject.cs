﻿using System;
using ScooterBear.GTD.Application.Projects;

namespace ScooterBear.GTD.AWS.DynamoDb.Project
{
    public class ReadOnlyProject : IProject
    {
        public string Id { get; }
        public string Name { get; }
        public string UserId { get; }
        public int Count { get; }
        public bool IsDeleted { get; }
        public int CountOverDue { get; }
        public int? VersionNumber { get; }
        public DateTime DateCreated { get; }

        public ReadOnlyProject(string id,string userId, string name, int count,  bool isDeleted, int countOverDue, int? versionNumber, DateTime dateCreated)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Count = count;
            IsDeleted = isDeleted;
            CountOverDue = countOverDue;
            VersionNumber = versionNumber;
            DateCreated = dateCreated;
        }
    }
}
