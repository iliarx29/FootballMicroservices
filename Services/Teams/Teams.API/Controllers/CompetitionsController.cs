using Microsoft.AspNetCore.Mvc;
using System.Net;
using Teams.API.Common;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;

namespace Teams.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetitionsController : ControllerBase
{
    private readonly ICompetitionService _competitionService;
    public CompetitionsController(ICompetitionService service)
    {
        _competitionService = service;
    }

    [HttpGet]
    public async Task<CustomActionResult<IEnumerable<CompetitionResponse>>> GetAllCompetitions()
    {
        var result = await _competitionService.GetAllCompetitionsAsync();

        if (!result.IsSuccess)
            return new CustomActionResult<IEnumerable<CompetitionResponse>>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<IEnumerable<CompetitionResponse>>(HttpStatusCode.OK, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<CustomActionResult<CompetitionResponse>> GetCompetitionById(Guid id)
    {
        var result = await _competitionService.GetCompetitionByIdAsync(id);

        if (!result.IsSuccess)
            return new CustomActionResult<CompetitionResponse>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<CompetitionResponse>(HttpStatusCode.OK, result.Value);
    }

    [HttpPost]
    public async Task<CustomActionResult<CompetitionResponse>> AddCompetition(CompetitionRequest competitionRequest)
    {
        var result = await _competitionService.AddCompetitionAsync(competitionRequest);

        if (!result.IsSuccess)
            return new CustomActionResult<CompetitionResponse>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<CompetitionResponse>(HttpStatusCode.Created, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<CustomActionResult> UpdateCompetition(Guid id, CompetitionRequest leagueRequest)
    {
        var result = await _competitionService.UpdateCompetitionAsync(id, leagueRequest);

        if (!result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult(HttpStatusCode.NoContent);
    }

    [HttpDelete("{id:guid}")]
    public async Task<CustomActionResult> DeleteCompetition(Guid id)
    {
        var result = await _competitionService.DeleteCompetitionAsync(id);

        if (!result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult(HttpStatusCode.NoContent);
    }

    [HttpGet("{id:guid}/teams")]
    public async Task<CustomActionResult<CompetitionResponse>> GetCompetitionWithTeams(Guid id)
    {
        var result = await _competitionService.GetCompetitionWithTeams(id);

        if (!result.IsSuccess)
            return new CustomActionResult<CompetitionResponse>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<CompetitionResponse>(HttpStatusCode.OK, result.Value);
    }

    [HttpPut("{id:guid}/teams")]
    public async Task<CustomActionResult<object>> AddTeamsToCompetition(Guid id, [FromBody] List<Guid> teamIds)
    {
        var result = await _competitionService.AddTeamsToCompetition(id, teamIds);

        if (!result.IsSuccess)
            return new CustomActionResult<object>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<object>(HttpStatusCode.OK, new { CountOfAddedTeamToCompetition = result.Value });
    }

    [HttpDelete("{id:guid}/teams")]
    public async Task<CustomActionResult<object>> RemoveTeamsFromCompetition(Guid id, [FromBody] List<Guid> teamsIds)
    {
        var result = await _competitionService.RemoveTeamsFromCompetition(id, teamsIds);

        if (!result.IsSuccess)
            return new CustomActionResult<object>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<object>(HttpStatusCode.OK, new { CountOfAddedTeamToCompetition = result.Value });
    }
}
