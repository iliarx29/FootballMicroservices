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
        var competitions = await _competitionService.GetAllCompetitionsAsync();

        return Ok(competitions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompetitionById(Guid id)
    {
        var competition = await _competitionService.GetCompetitionByIdAsync(id);

        return Ok(competition);
    }

    [HttpPost]
    public async Task<IActionResult> AddCompetition(CompetitionRequest competitionRequest)
    {
        var competitionResponse = await _competitionService.AddCompetitionAsync(competitionRequest);

        return CreatedAtAction(nameof(GetCompetitionById), new { competitionResponse.Id }, competitionResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompetition(Guid id, CompetitionRequest leagueRequest)
    {
        await _competitionService.UpdateCompetitionAsync(id, leagueRequest);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompetition(Guid id)
    {
        await _competitionService.DeleteCompetitionAsync(id);

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
