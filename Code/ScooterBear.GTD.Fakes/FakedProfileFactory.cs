using System;
using ScooterBear.GTD.Application.UserProfile;

namespace ScooterBear.GTD.Fakes
{
    public class FakedProfileFactory : IProfileFactory
    {
        public Profile Profile { get; private set; }

        public Profile GetCurrentProfile()
        {
            return Profile;
        }

        public void SetUserProfile(Profile profile)
        {
            Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }
    }
}