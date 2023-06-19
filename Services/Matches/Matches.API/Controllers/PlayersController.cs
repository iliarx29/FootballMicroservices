using Matches.API.Common;
using Matches.Application.Players.Commands;
using Matches.Application.Players.Queries;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    public async Task<CustomActionResult> GetPlayers()
    {
        var players = await _mediator.Send(new GetPlayersQuery());

        if (players.IsSuccess)
            return new CustomActionResult<IEnumerable<Player>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<IEnumerable<Player>>(players.Value);

        return new CustomActionResult<Player>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Player>(ErrorCode.NotFound, players.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<CustomActionResult> GetPlayerById(Guid id)
    {
        var player = await _mediator.Send(new GetPlayerByIdQuery(id));

        if (player.IsSuccess)
            return new CustomActionResult<Player>(HttpStatusCode.OK, ErrorCode.OK).Success<Player>(player.Value);

        return new CustomActionResult<Player>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Player>(ErrorCode.NotFound, player.ErrorMessage);
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<CustomActionResult> GetPlayersByTeamId(Guid teamId)
    {
        var players = await _mediator.Send(new GetPlayersByTeamIdQuery(teamId));

        if (players.IsSuccess)
            return new CustomActionResult<IEnumerable<Player>>(HttpStatusCode.OK, ErrorCode.OK).Success<IEnumerable<Player>>(players.Value);

        return new CustomActionResult<Player>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Player>(ErrorCode.NotFound, players.ErrorMessage);
    }

    //[Authorize("write_access")]
    [HttpPost]
    public async Task<IActionResult> CreatePlayer(CreatePlayerCommand command)
    {
        var player = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetPlayerById), new { player.Value.Id }, player);
    }

    //[Authorize("write_access")]
    [HttpPost("importPlayers")]
    public async Task<CustomActionResult> ImportPlayers()
    {
        var res = await _mediator.Send(new ImportPlayersCommand());

        if (res.IsSuccess)
            return new CustomActionResult<int>(HttpStatusCode.OK, ErrorCode.OK).Success<int>(res.Value);

        return new CustomActionResult<int>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<int>(ErrorCode.NotFound, res.ErrorMessage);
    }
}
