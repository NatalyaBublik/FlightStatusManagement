using FlightStatusManagement.Api.Flights.Dtos;
using FlightStatusManagement.Application.Flights.Commands.CreateFlight;
using FlightStatusManagement.Application.Flights.Commands.UpdateFlightStatus;
using FlightStatusManagement.Application.Flights.Queries.GetFlights;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightStatusManagement.Api.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ISender _sender;

        public FlightsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] string? origin, [FromQuery] string? destination, CancellationToken ct)
        {
            var result = await _sender.Send(new GetFlightsQuery(origin, destination), ct);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([FromBody] CreateFlightCommand command, CancellationToken ct)
        {
            var id = await _sender.Send(command, ct);

            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateStatusRequest body, CancellationToken ct)
        {
            await _sender.Send(new UpdateFlightStatusCommand(id, body.Status), ct);

            return NoContent();
        }
    }
}
