using System;

namespace ScooterBear.GTD.Application.UserProfile
{
    public class Profile
    {
        public string UserId { get; }

        public Profile(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }
    }
}
