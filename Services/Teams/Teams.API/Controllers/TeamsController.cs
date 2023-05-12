using Microsoft.AspNetCore.Mvc;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;

namespace Teams.API.Controllers;
[Route("api/teams")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService service)
    {
        _teamService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _teamService.GetAllTeamsAsync();

        return Ok(teams);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeamById(Guid id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);

        return Ok(team);
    }

    [HttpPost]
    public async Task<IActionResult> AddTeam(TeamRequest teamRequest)
    {
        var teamResponse = await _teamService.AddTeamAsync(teamRequest);

        return CreatedAtAction(nameof(GetTeamById), new { teamResponse.Id}, teamResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeam(Guid id, TeamRequest teamRequest)
    {
        await _teamService.UpdateTeamAsync(id, teamRequest);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        await _teamService.DeleteTeamAsync(id);

        return NoContent();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportTeams()
    {
        var result = await _teamService.ImportTeams();

        return Ok(result);

    }
}
