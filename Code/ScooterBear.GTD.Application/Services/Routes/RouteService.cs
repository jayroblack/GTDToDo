using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Routes
{
    public class RouteService : IService<RouteServiceArgs, RouteServiceResult>
    {
        private readonly IRouteConfiguration _routeConfiguration;

        public RouteService(IRouteConfiguration routeConfiguration)
        {
            _routeConfiguration = routeConfiguration ?? throw new ArgumentNullException(nameof(routeConfiguration));
        }
        public async Task<RouteServiceResult> Run(RouteServiceArgs arg)
        {
            var root = $"{_routeConfiguration.Scheme}://{_routeConfiguration.Domain}";
            if (arg.RouteName == RouteName.ValidateEmail)
                return new RouteServiceResult(string.Concat(root, "/", "user/validateEmail"));
            throw new NotImplementedException();
        }
    }
}
