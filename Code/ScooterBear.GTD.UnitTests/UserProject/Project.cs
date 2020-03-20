using System;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProject;
using Xunit;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheProjectI
    {
        [Theory]
        [InlineData(null, "Name", "UserId", 0, false, 0, false)]
        [InlineData("", "Name", "UserId", 0, false, 0, false)]
        [InlineData("ProjectId", null, "UserId", 0, false, 0, false)]
        [InlineData("ProjectId", "", "UserId", 0, false, 0, false)]
        [InlineData("ProjectId", "Name", null, 0, false, 0, false)]
        [InlineData("ProjectId", "Name", "", 0, false, 0, false)]
        [InlineData("ProjectId", "Name", "UserId", -1, false, 0, false)]
        [InlineData("ProjectId", "Name", "UserId", 0, false, -1, false)]
        [InlineData("ProjectId", "Name", "UserId", 0, false, 1, true)]
        public void ShouldNotAllowInvalidToExist(string projectId, string projectName, string userId,
            int count, bool isDeleted, int countOverdue, bool shouldPass)
        {
            var action = new Action(() => new Project(projectId, projectName,
                userId, count, countOverdue, DateTime.UtcNow, 0, isDeleted));

            if (shouldPass)
                action();
            else
                action.Should().Throw<ArgumentException>();
        }
    }
}