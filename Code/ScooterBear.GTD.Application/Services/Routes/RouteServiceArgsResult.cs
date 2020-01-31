using System;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Services.Routes
{
    public class RouteServiceResult : IServiceResult
    {
        public string Url { get; }

        public RouteServiceResult(string url)
        {
            if( string.IsNullOrEmpty(url))
                throw new ArgumentException($"{nameof(url)} is required.");
            Url = url;
        }
    }

    public class RouteServiceArgs : IServiceArgs<RouteServiceResult>
    {
        public RouteName RouteName { get; }

        public RouteServiceArgs(RouteName routeName)
        {
            RouteName = routeName;
        }
    }

    public enum RouteName
    {
        ValidateEmail
    }
}
