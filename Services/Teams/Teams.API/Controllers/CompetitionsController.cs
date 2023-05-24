using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAllCompetitions()
    {
        var result = await _competitionService.GetAllCompetitionsAsync();

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompetitionById(Guid id)
    {
        var result = await _competitionService.GetCompetitionByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AddCompetition(CompetitionRequest competitionRequest)
    {
        var result = await _competitionService.AddCompetitionAsync(competitionRequest);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return CreatedAtAction(nameof(GetCompetitionById), new { result.Value?.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompetition(Guid id, CompetitionRequest leagueRequest)
    {
        var result = await _competitionService.UpdateCompetitionAsync(id, leagueRequest);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompetition(Guid id)
    {
        var result = await _competitionService.DeleteCompetitionAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return NoContent();
    }

    [HttpGet("{id:guid}/teams")]
    public async Task<IActionResult> GetCompetitionWithTeams(Guid id)
    {
        var result = await _competitionService.GetCompetitionWithTeams(id);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/teams")]
    public async Task<IActionResult> AddTeamsToCompetition(Guid id, [FromBody] List<Guid> teamIds)
    {
        var result = await _competitionService.AddTeamsToCompetition(id, teamIds);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(new { CountOfAddedTeamToCompetition = result.Value });
    }

    [HttpDelete("{id:guid}/teams")]
    public async Task<IActionResult> RemoveTeamsFromCompetition(Guid id, [FromBody] List<Guid> teamsIds)
    {
        var result = await _competitionService.RemoveTeamsFromCompetition(id, teamsIds);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(new { CountOfRemovedTeamFromCompetition = result.Value });
    }
}
