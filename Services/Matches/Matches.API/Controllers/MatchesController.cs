using Hangfire;
using Matches.API.Common;
using Matches.Application;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.DeleteMatch;
using Matches.Application.Matches.Commands.ImportMatches;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Matches.Queries.GetH2HMatches;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
using Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
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
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ImportDataRecurringJob _importJob;

    public MatchesController(IMediator context, IRecurringJobManager recurringJobManager, ImportDataRecurringJob importJob)
    {
        _mediator = context;
        _recurringJobManager = recurringJobManager;
        _importJob = importJob;
    }

    [HttpGet]
    public async Task<CustomActionResult> GetAllMatches()
    {
        var matches = await _mediator.Send(new GetMatchesQuery());

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<Match>>(HttpStatusCode.OK, ErrorCode.OK).Success<IEnumerable<Match>>(matches.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<CustomActionResult> GetMatchById(Guid id)
    {
        var match = await _mediator.Send(new GetMatchByIdQuery(id));

        if (match.IsSuccess)
            return new CustomActionResult<Match>(HttpStatusCode.OK, ErrorCode.OK).Success<Match>(match.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, match.ErrorMessage);
    }

    [HttpGet("competitions/{competitionId:guid}")]
    public async Task<CustomActionResult> GetMatchesByCompetitionId(Guid competitionId)
    {
        var matches = await _mediator.Send(new GetMatchesByCompetitionIdQuery(competitionId));

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<Match>>(HttpStatusCode.OK, ErrorCode.OK).Success<IEnumerable<Match>>(matches.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}/h2h")]
    public async Task<CustomActionResult> GetH2HMatches(Guid id)
    {
        var matches = await _mediator.Send(new GetH2HMatchesQuery(id));

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<Match>>(HttpStatusCode.OK, ErrorCode.OK).Success<IEnumerable<Match>>(matches.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpPost]
    public async Task<CustomActionResult> AddMatch(CreateMatchCommand command)
    {
        var match = await _mediator.Send(command);

        if (match.IsSuccess)
            return new CustomActionResult<Match>(HttpStatusCode.Created, ErrorCode.OK).Success<Match>(match.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, match.ErrorMessage);
    }

    [HttpGet("competition/{competitionId:guid}/standings")]
    public async Task<CustomActionResult> GetStandingsByLeagueId(Guid competitionId, [FromQuery] string season)
    {
        var standings = await _mediator.Send(new GetStandingsByCompetitionAndSeasonQuery(competitionId, season));

        if (standings.IsSuccess)
            return new CustomActionResult<IEnumerable<Ranking>>(HttpStatusCode.OK, ErrorCode.OK).Success<IEnumerable<Ranking>>(standings.Value);

        return new CustomActionResult<IEnumerable<Ranking>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Ranking>(ErrorCode.NotFound, standings.ErrorMessage);
    }

    [HttpDelete("{id:guid}")]
    public async Task<CustomActionResult> DeleteMatch(Guid id)
    {
        var result = await _mediator.Send(new DeleteMatchCommand(id));

        if (result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.OK, ErrorCode.OK).Success();

        return new CustomActionResult(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [HttpPost("competitions/{competitionId:guid}/import")]
    public async Task<CustomActionResult> ImportMatches(Guid competitionId, [FromQuery] string season)
    {
        var result = await _mediator.Send(new ImportMatchesCommand(competitionId, season));

        if (result.IsSuccess)
            return new CustomActionResult<int>(HttpStatusCode.OK, ErrorCode.OK).Success<int>(result.Value);

        return new CustomActionResult<int>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<CustomActionResult> UpdateMatch(UpdateMatchCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.OK, ErrorCode.OK).Success();

        return new CustomActionResult(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [HttpPost("competitions/{competitionId:guid}/jobImport")]
    public IActionResult ImportJob(Guid competitionId, [FromQuery] string season)
    {
        _recurringJobManager.AddOrUpdate("importJob", () => _importJob.ImportMatches(competitionId, season), Cron.Daily(19));
        return NoContent();
    }
}
