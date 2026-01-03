using System.Security.Claims;
using FlightStatusManagement.Application.Common.Interfaces;

namespace FlightStatusManagement.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUserService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public int? UserId
        {
            get
            {
                var sub = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? _http.HttpContext?.User?.FindFirstValue("sub");

                return int.TryParse(sub, out var id) ? id : null;
            }
        }

        public string? Username => _http.HttpContext?.User?.Identity?.Name;
    }
}
