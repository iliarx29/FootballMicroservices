using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.DeleteMatch;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Matches.Queries.GetMatchesByLeagueId;
using Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matches.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchesController(IMediator context)
    {
        _mediator = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMatches()
    {
        var matches = await _mediator.Send(new GetMatchesQuery());

        return Ok(matches);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMatchById(Guid id)
    {
        var match = await _mediator.Send(new GetMatchByIdQuery(id));

        return Ok(match);
    }

    [HttpGet("leagues/{leagueId:guid}")]
    public async Task<IActionResult> GetMatchesByLeagueId(Guid leagueId)
    {
        var matches = await _mediator.Send(new GetMatchesByLeagueIdQuery(leagueId));

        return Ok(matches);
    }

    [HttpPost]
    public async Task<IActionResult> AddMatch(CreateMatchCommand command)
    {
        var match = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetMatchById), new { match.Id }, command);
    }

    [HttpGet("leagues/{leagueId:guid}/standings")]
    public async Task<IActionResult> GetStandingsByLeagueId(Guid leagueId, [FromQuery] Guid seasonId)
    {
        var standings = await _mediator.Send(new GetStandingsByLeagueAndSeasonQuery(leagueId, seasonId));

        return Ok(standings);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        await _mediator.Send(new DeleteMatchCommand(id));

        return NoContent();
    }
}
