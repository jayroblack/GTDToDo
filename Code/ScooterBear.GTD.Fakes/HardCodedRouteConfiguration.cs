using ScooterBear.GTD.Application.Services.Routes;

namespace ScooterBear.GTD.Fakes
{
    public class HardCodedRouteConfiguration : IRouteConfiguration
    {
        public string Scheme
        {
            get { return "http"; }
        }
        public string Domain
        {
            get { return "website.com"; }
        }
    }
}
