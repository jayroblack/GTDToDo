using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public class NewUserProjectServiceResult : IServiceResult
    {
    }

    public class NewUserProjectServiceArg : IServiceArgs<NewUserProjectServiceResult>
    {
        public string UserId { get; }
        public string ProjectName { get; }

        public NewUserProjectServiceArg(string userId, string projectName)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"{nameof(userId)} is required.");
            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentException($"{nameof(projectName)} is required.");
            UserId = userId;
            ProjectName = projectName;
        }
    }
}
