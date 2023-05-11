using Matches.Application.Players.Commands;
using Matches.Application.Players.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Matches.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _mediator.Send(new GetPlayersQuery());

        return Ok(players);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlayerById(Guid id)
    {
        var players = await _mediator.Send(new GetPlayerByIdQuery(id));

        return Ok(players);
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<IActionResult> GetPlayersByTeamId(Guid teamId)
    {
        var players = await _mediator.Send(new GetPlayersByTeamIdQuery(teamId));

        return Ok(players);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer(CreatePlayerCommand command)
    {
        var player = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetPlayerById), new { player.Id }, player);
    }
}
