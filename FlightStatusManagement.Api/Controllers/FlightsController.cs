using FlightStatusManagement.Api.Flights.Dtos;
using FlightStatusManagement.Application.Flights.Commands.CreateFlight;
using FlightStatusManagement.Application.Flights.Commands.UpdateFlightStatus;
using FlightStatusManagement.Application.Flights.Queries.GetFlights;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightStatusManagement.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing flight information.
    /// </summary>
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ISender _sender;

        public FlightsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Retrieves a list of all flights sorted by arrival time.
        /// </summary>
        /// <param name="origin">
        /// Optional filter by flight origin.
        /// </param>
        /// <param name="destination">
        /// Optional filter by flight destination.
        /// </param>
        /// <response code="200">Flights successfully retrieved.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] string? origin, [FromQuery] string? destination, CancellationToken ct)
        {
            var result = await _sender.Send(new GetFlightsQuery(origin, destination), ct);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new flight.
        /// </summary>
        /// <param name="command">
        /// Flight details including route, schedule and initial status.
        /// </param>
        /// <response code="201">Flight successfully created.</response>
        /// <response code="400">Invalid flight data.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([FromBody] CreateFlightCommand command, CancellationToken ct)
        {
            var id = await _sender.Send(command, ct);

            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        /// <summary>
        /// Updates the status of an existing flight.
        /// </summary>
        /// <param name="id">
        /// Unique identifier of the flight.
        /// </param>
        /// <param name="body">
        /// New flight status.
        /// </param>
        /// <response code="204">Flight status successfully updated.</response>
        /// <response code="400">Invalid status value.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Flight with the specified ID was not found.</response>
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateStatusRequest body, CancellationToken ct)
        {
            await _sender.Send(new UpdateFlightStatusCommand(id, body.Status), ct);

            return NoContent();
        }
    }
}
