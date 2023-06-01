using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Teams.API.Common;
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
    public async Task<CustomActionResult<IEnumerable<TeamResponse>>> GetAllTeams()
    {
        var result = await _teamService.GetAllTeamsAsync();

        if (!result.IsSuccess)
            return new CustomActionResult<IEnumerable<TeamResponse>>(HttpStatusCode.NotFound, result.ErrorMessage);
            
        return new CustomActionResult<IEnumerable<TeamResponse>>(HttpStatusCode.OK, result.Value);
    }

    [Authorize(Roles = "User")]
    [HttpGet("{id:guid}")]
    public async Task<CustomActionResult<TeamResponse>> GetTeamById(Guid id)
    {
        var result = await _teamService.GetTeamByIdAsync(id);

        if (!result.IsSuccess)
        {
            return new CustomActionResult<TeamResponse>(HttpStatusCode.NotFound, result.ErrorMessage);
        };

        return new CustomActionResult<TeamResponse>(HttpStatusCode.OK, result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<CustomActionResult<TeamResponse>> AddTeam(TeamRequest teamRequest)
    {
        var result = await _teamService.AddTeamAsync(teamRequest);

        if (!result.IsSuccess)
        {
            return new CustomActionResult<TeamResponse>(HttpStatusCode.NotFound, result.ErrorMessage);
        };

        //return CreatedAtAction(nameof(GetTeamById), new { result.Value?.Id }, result.Value);

        return new CustomActionResult<TeamResponse>(HttpStatusCode.Created, result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<CustomActionResult> UpdateTeam(Guid id, TeamRequest teamRequest)
    {
        var result = await _teamService.UpdateTeamAsync(id, teamRequest);

        if (!result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.NotFound, result.ErrorMessage);
            
        return new CustomActionResult(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<CustomActionResult> DeleteTeam(Guid id)
    {
        var result = await _teamService.DeleteTeamAsync(id);

        if (!result.IsSuccess)
            return new CustomActionResult(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult(HttpStatusCode.NoContent);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("import")]
    public async Task<CustomActionResult<object>> ImportTeams()
    {
        var result = await _teamService.ImportTeams();

        if (!result.IsSuccess)
            return new CustomActionResult<object>(HttpStatusCode.NotFound, result.ErrorMessage);

        return new CustomActionResult<object>(HttpStatusCode.OK, new {CountOfAddedTeams = result.Value});

    }
}
