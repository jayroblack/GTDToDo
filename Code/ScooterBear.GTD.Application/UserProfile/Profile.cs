using System;

namespace ScooterBear.GTD.Application.UserProfile
{
    public class Profile
    {
        public Profile(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        public string UserId { get; }
    }
}