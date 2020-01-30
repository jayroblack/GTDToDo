using System;

namespace ScooterBear.GTD.Application.Services.Security
{
    public class EmailUserToken
    {
        public DateTime Created { get; set; }
        public int Version { get; set; }
        public string UserId { get; set; }
    }
}
