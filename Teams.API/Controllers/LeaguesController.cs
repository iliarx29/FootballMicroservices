using Microsoft.AspNetCore.Mvc;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;

namespace Teams.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LeaguesController : ControllerBase
{
    private readonly ILeagueService _leagueService;
    public LeaguesController(ILeagueService service)
    {
        _leagueService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeagues()
    {
        var leagues = await _leagueService.GetAllLeaguesAsync();

        return Ok(leagues);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLeagueById(Guid id)
    {
        var league = await _leagueService.GetLeagueByIdAsync(id);

        return Ok(league);
    }

    [HttpPost]
    public async Task<IActionResult> AddLeague(LeagueRequest leagueRequest)
    {
        var leagueResponse = await _leagueService.AddLeagueAsync(leagueRequest);

        return CreatedAtAction(nameof(GetLeagueById), new { leagueResponse.Id }, leagueResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateLeague(Guid id, LeagueRequest leagueRequest)
    {
        await _leagueService.UpdateLeagueAsync(id, leagueRequest);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        await _leagueService.DeleteLeagueAsync(id);

        return NoContent();
    }

    [HttpGet("{id:guid}/teams")]
    public async Task<IActionResult> GetTeamsOfLeague(Guid id)
    {
        var leagueResponse = await _leagueService.GetTeamsOfLeagueAsync(id);

        return Ok(leagueResponse);
    }
}
