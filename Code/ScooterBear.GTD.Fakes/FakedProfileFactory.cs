using System;
using System.Collections.Generic;
using System.Text;
using ScooterBear.GTD.Application.UserProfile;

namespace ScooterBear.GTD.Fakes
{
    public class FakedProfileFactory : IProfileFactory
    {
        public Profile Profile { get; private set; }

        public void SetUserProfile(Profile profile)
        {
            this.Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public Profile GetCurrentProfile()
        {
            return this.Profile;
        }
    }
}
