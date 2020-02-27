using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ScooterBear.GTD.Application.UserProfile;

namespace ScooterBear.GTD.Profile
{
    public class HttpContextProfileFactory : IProfileFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextProfileFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Application.UserProfile.Profile GetCurrentProfile()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext.User.Claims.FirstOrDefault(x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return new Application.UserProfile.Profile(userId.Value);
        }
    }
}
