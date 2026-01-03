using FlightStatusManagement.Application.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightStatusManagement.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);

            return Ok(result);
        }
    }
}
