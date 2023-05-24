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
        var result = await _teamService.GetAllTeamsAsync();

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeamById(Guid id)
    {
        var result = await _teamService.GetTeamByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AddTeam(TeamRequest teamRequest)
    {
        var result = await _teamService.AddTeamAsync(teamRequest);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return CreatedAtAction(nameof(GetTeamById), new { result.Value?.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeam(Guid id, TeamRequest teamRequest)
    {
        var result = await _teamService.UpdateTeamAsync(id, teamRequest);

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var result = await _teamService.DeleteTeamAsync(id);

        if(!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return NoContent();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportTeams()
    {
        var result = await _teamService.ImportTeams();

        if (!result.IsSuccess)
            return NotFound(new { result.IsSuccess, result.ErrorMessage, result.ErrorCode });

        return Ok(result.Value);

    }
}
