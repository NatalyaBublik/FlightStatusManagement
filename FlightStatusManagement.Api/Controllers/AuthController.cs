using FlightStatusManagement.Application.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightStatusManagement.Api.Controllers
{
    /// <summary>
    /// Provides authentication endpoints.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT access token.
        /// </summary>
        /// <param name="command">
        /// User credentials containing username and password.
        /// </param>
        /// <response code="200">Authentication succeeded.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Invalid username or password.</response>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);

            return Ok(result);
        }
    }
}
