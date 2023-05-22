using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.DeleteMatch;
using Matches.Application.Matches.Commands.ImportMatches;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Matches.Queries.GetH2HMatches;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
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

    [HttpGet("competitions/{competitionId:guid}")]
    public async Task<IActionResult> GetMatchesByCompetitionId(Guid competitionId)  
    {
        var matches = await _mediator.Send(new GetMatchesByCompetitionIdQuery(competitionId));

        return Ok(matches);
    }

    [HttpGet("{id:guid}/h2h")]
    public async Task<IActionResult> GetH2HMatches(Guid id)
    {
        var matches = await _mediator.Send(new GetH2HMatchesQuery(id));

        return Ok(matches);
    }

    [HttpPost]
    public async Task<IActionResult> AddMatch(CreateMatchCommand command)
    {
        var match = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetMatchById), new { match.Id }, match);
    }

    [HttpGet("competitions/{leagueId:guid}/standings")]
    public async Task<IActionResult> GetStandingsByLeagueId(Guid leagueId, [FromQuery] string season)
    {
        var standings = await _mediator.Send(new GetStandingsByLeagueAndSeasonQuery(leagueId, season));

        return Ok(standings);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        await _mediator.Send(new DeleteMatchCommand(id));

        return NoContent();
    }

    [HttpPost("competitions/{competitionId:guid}/import")]
    public async Task<IActionResult> ImportMatches(Guid competitionId, [FromQuery] string season)
    {
        var result = await _mediator.Send(new ImportMatchesCommand(competitionId, season));

        return Ok(new { CountOfMatches = result });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMatch(UpdateMatchCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }
}
