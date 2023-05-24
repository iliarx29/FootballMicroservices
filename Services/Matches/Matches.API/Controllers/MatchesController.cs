using Matches.API.Common;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.DeleteMatch;
using Matches.Application.Matches.Commands.ImportMatches;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Matches.Queries.GetH2HMatches;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Matches.Queries.GetMatchesByLeagueId;
using Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    public async Task<MatchesActionResult> GetAllMatches()
    {
        var matches = await _mediator.Send(new GetMatchesQuery());

        if (matches.IsSuccess)
            return new MatchesActionResult<IEnumerable<Match>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<IEnumerable<Match>>(matches.Value);

        return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<MatchesActionResult> GetMatchById(Guid id)
    {
        var match = await _mediator.Send(new GetMatchByIdQuery(id));

        if (match.IsSuccess)
            return new MatchesActionResult<Match>(HttpStatusCode.OK, ErrorCode.OK).Success<Match>(match.Value);

        return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, match.ErrorMessage);
    }

    [HttpGet("leagues/{leagueId:guid}")]
    public async Task<MatchesActionResult> GetMatchesByLeagueId(Guid leagueId)
    {
        var matches = await _mediator.Send(new GetMatchesByLeagueIdQuery(leagueId));

        if (matches.IsSuccess)
            return new MatchesActionResult<IEnumerable<Match>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<IEnumerable<Match>>(matches.Value);

        return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}/h2h")]
    public async Task<MatchesActionResult> GetH2HMatches(Guid id)
    {
        var matches = await _mediator.Send(new GetH2HMatchesQuery(id));

        if (matches.IsSuccess)
            return new MatchesActionResult<IEnumerable<Match>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<IEnumerable<Match>>(matches.Value);

        return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpPost]
    public async Task<MatchesActionResult> AddMatch(CreateMatchCommand command)
    {
        var match = await _mediator.Send(command);

        if (match.IsSuccess)
            return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<Match>(match.Value);

        return new MatchesActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, match.ErrorMessage);
    }

    [HttpGet("leagues/{leagueId:guid}/standings")]
    public async Task<MatchesActionResult> GetStandingsByLeagueId(Guid leagueId, [FromQuery] Guid seasonId)
    {
        var standings = await _mediator.Send(new GetStandingsByLeagueAndSeasonQuery(leagueId, seasonId));

        if (standings.IsSuccess)
            return new MatchesActionResult<IEnumerable<Ranking>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Success<IEnumerable<Ranking>>(standings.Value);

        return new MatchesActionResult<IEnumerable<Ranking>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Ranking>(ErrorCode.NotFound, standings.ErrorMessage);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        await _mediator.Send(new DeleteMatchCommand(id));

        return NoContent();
    }

    [HttpPost("leagues/{leagueId:guid}/import")]
    public async Task<IActionResult> ImportMatches(Guid leagueId, [FromQuery] Guid seasonId)
    {
        var result = await _mediator.Send(new ImportMatchesCommand(leagueId, seasonId));

        return Ok(new { CountOfMatches = result });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMatch(UpdateMatchCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }
}
