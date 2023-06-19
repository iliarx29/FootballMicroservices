using Hangfire;
using Matches.API.Common;
using Matches.Application;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.DeleteMatch;
using Matches.Application.Matches.Commands.ImportMatches;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Matches.Queries.AutocompleteSearch;
using Matches.Application.Matches.Queries.GetH2HMatches;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
using Matches.Application.Matches.Queries.GetMatchesByTeamId;
using Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
using Matches.Application.Matches.Queries.SearchMatches;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
            return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.OK, ErrorCode.OK).Success(matches.Value);

        return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<CustomActionResult> GetMatchById(Guid id)
    {
        var match = await _mediator.Send(new GetMatchByIdQuery(id));

        if (match.IsSuccess)
            return new CustomActionResult<MatchResponse>(HttpStatusCode.OK, ErrorCode.OK).Success(match.Value);

        return new CustomActionResult<MatchResponse>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, match.ErrorMessage);
    }

    [HttpGet("competitions/{competitionId:guid}")]
    public async Task<CustomActionResult> GetMatchesByCompetitionId(Guid competitionId)
    {
        var matches = await _mediator.Send(new GetMatchesByCompetitionIdQuery(competitionId));

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.OK, ErrorCode.OK).Success(matches.Value);

        return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<CustomActionResult> GetMatchesByTeamId(Guid teamId)
    {
        var matches = await _mediator.Send(new GetMatchesByTeamIdQuery(teamId));

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.OK, ErrorCode.OK).Success(matches.Value);

        return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("{id:guid}/h2h")]
    public async Task<CustomActionResult> GetH2HMatches(Guid id)
    {
        var matches = await _mediator.Send(new GetH2HMatchesQuery(id));

        if (matches.IsSuccess)
            return new CustomActionResult<IEnumerable<MatchResponse>>(HttpStatusCode.OK, ErrorCode.OK).Success(matches.Value);

        return new CustomActionResult<MatchResponse>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, matches.ErrorMessage);
    }

    [HttpGet("competition/{competitionId:guid}/standings")]
    public async Task<CustomActionResult> GetStandingsByCompetitionId(Guid competitionId, [FromQuery] string season)
    {
        var standings = await _mediator.Send(new GetStandingsByCompetitionAndSeasonQuery(competitionId, season));

        if (standings.IsSuccess)
            return new CustomActionResult<IEnumerable<Ranking>>(HttpStatusCode.OK, ErrorCode.OK).Success(standings.Value);

        return new CustomActionResult<IEnumerable<Ranking>>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, standings.ErrorMessage);
    }

    [Authorize("write_access")]
    [HttpPost]
    public async Task<CustomActionResult> AddMatch(CreateMatchCommand command)
    {
        var match = await _mediator.Send(command);

        if (match.IsSuccess)
            return new CustomActionResult<Match>(HttpStatusCode.Created, ErrorCode.OK).Success(match.Value);

        return new CustomActionResult<Match>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail<Match>(ErrorCode.NotFound, match.ErrorMessage);
    }

    [Authorize("write_access")]
    [HttpDelete("{id:guid}")]
    public async Task<CustomActionResult> DeleteMatch(Guid id)
    {
        var result = await _mediator.Send(new DeleteMatchCommand(id));

        if (result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.OK, ErrorCode.OK).Success();

        return new CustomActionResult(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [Authorize("write_access")]
    [HttpPost("competitions/{competitionId:guid}/import")]
    public async Task<CustomActionResult> ImportMatches(Guid competitionId, [FromQuery] string season, IFormFile file)
    {
        var result = await _mediator.Send(new ImportMatchesCommand(competitionId, season, file));

        if (result.IsSuccess)
            return new CustomActionResult<int>(HttpStatusCode.OK, ErrorCode.OK).Success<int>(result.Value);

        return new CustomActionResult<int>(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [Authorize("write_access")]
    [HttpPut("{id:guid}")]
    public async Task<CustomActionResult> UpdateMatch(UpdateMatchCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.OK, ErrorCode.OK).Success();

        return new CustomActionResult(HttpStatusCode.NotFound, ErrorCode.NotFound).Fail(ErrorCode.NotFound, result.ErrorMessage);
    }

    [Authorize("write_access")]
    [HttpPost("competitions/{competitionId:guid}/jobImport")]
    public IActionResult ImportJob(Guid competitionId, [FromQuery] string season)
    {
        _recurringJobManager.AddOrUpdate("importJob", () => _importJob.ImportMatches(competitionId, season), Cron.Daily(19));
        return NoContent();
    }

    [HttpGet("search/{value}")]
    public async Task<IActionResult> Search(string value)
    {
        var searchQuery = new SearchMatchesQuery(value);

        var response = await _mediator.Send(searchQuery);

        return Ok(response);
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromQuery]string value)
    {
        var response = await _mediator.Send(new AutocompleteSearchQuery(value));

        return Ok(response);
    }
}
