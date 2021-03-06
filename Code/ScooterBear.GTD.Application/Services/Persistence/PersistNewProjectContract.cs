﻿using System;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Persistence
{
    public class PersistNewProjectResult : IServiceResult
    {
        public IProject Project { get; }

        public PersistNewProjectResult(IProject project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    public class PersistProjectArg : IServiceArgs<PersistNewProjectResult>, IServiceArgs<PersistUpdateProjectResult>
    {
        public string Id { get; }
        public string UserId { get; }
        public string ProjectName { get; }
        public DateTime DateTimeCreated { get; }
        public bool ConsistentRead { get; }

        public PersistProjectArg(string id, string userId, string projectName, DateTime dateTimeCreated,
            bool consistentRead = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");

            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentException($"{nameof(projectName)} is required.");

            if (dateTimeCreated == default)
                throw new ArgumentException($"{nameof(dateTimeCreated)} is required.");

            Id = id;
            UserId = userId;
            ProjectName = projectName;
            DateTimeCreated = dateTimeCreated;
            ConsistentRead = consistentRead;
        }
    }
}