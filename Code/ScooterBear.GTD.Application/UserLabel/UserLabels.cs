using System;
using System.Collections.Generic;

namespace ScooterBear.GTD.Application.UserLabel
{
    public class UserLabels
    {
        public string UserId { get; }
        public IEnumerable<ILabel> Labels { get; }

        public UserLabels(string userId, IEnumerable<ILabel> projects)
        {
            UserId = userId;
            Labels = projects ?? throw new ArgumentNullException(nameof(projects));
        }
    }
}
