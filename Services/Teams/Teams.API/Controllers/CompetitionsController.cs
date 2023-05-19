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
    public async Task<IActionResult> GetAllLeagues()
    {
        var leagues = await _competitionService.GetAllLeaguesAsync();

        return Ok(leagues);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLeagueById(Guid id)
    {
        var league = await _competitionService.GetLeagueByIdAsync(id);

        return Ok(league);
    }

    [HttpPost]
    public async Task<IActionResult> AddLeague(CompetitionRequest competitionRequest)
    {
        var competitionResponse = await _competitionService.AddCompetitionAsync(competitionRequest);

        return CreatedAtAction(nameof(GetLeagueById), new { competitionResponse.Id }, competitionResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateLeague(Guid id, CompetitionRequest leagueRequest)
    {
        await _competitionService.UpdateLeagueAsync(id, leagueRequest);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        await _competitionService.DeleteLeagueAsync(id);

        return NoContent();
    }

    [HttpGet("{id:guid}/teams")]
    public async Task<IActionResult> GetCompetitionWithTeams(Guid id)
    {
        var competitionResponse = await _competitionService.GetCompetitionWithTeams(id);

        return Ok(competitionResponse);
    }

    [HttpPost("{id:guid}/teams")]
    public async Task<IActionResult> AddTeamsToCompetition(Guid id, [FromBody] List<Guid> teamIds)
    {
       var result = await _competitionService.AddTeamsToCompetition(id, teamIds);

        return Ok(new {CountOfAddedTeamToCompetition = result});
    }
}
