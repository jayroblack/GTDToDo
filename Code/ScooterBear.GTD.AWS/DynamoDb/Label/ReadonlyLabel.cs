﻿using System;
using ScooterBear.GTD.Application.Label;

namespace ScooterBear.GTD.AWS.DynamoDb.Label
{
    public class ReadonlyLabel : ILabel
    {
        public string Id { get; }
        public string Name { get; }
        public string UserId { get; }
        public int Count { get; }
        public bool IsDeleted { get; }
        public int CountOverDue { get; }
        public int? VersionNumber { get; }
        public DateTime DateCreated { get; }

        public ReadonlyLabel(string id, string userId, string name, int count, bool isDeleted, int countOverDue, int? versionNumber, DateTime dateCreated)
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