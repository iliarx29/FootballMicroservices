using Matches.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Matches.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MatchesController : ControllerBase
{
    private readonly MatchesDbContext _context;

    public MatchesController(MatchesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMatches()
    {
        var matches = await _context.Matches.ToListAsync();

        return Ok(matches);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMatchById(Guid id)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == id);

        return Ok(match);
    }

    [HttpGet("leagues/{leagueId:guid}")]
    public async Task<IActionResult> GetMatchesByLeagueId(Guid leagueId)
    {
        var matches = await _context.Matches
            .Include(x => x.League)
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .Where(x => x.LeagueId == leagueId)
            .Select(x => new { x.Id, x.MatchDate, League = x.League.Name, x.Status, HomeTeam = x.HomeTeam.Name, x.HomeGoals, x.AwayGoals, AwayTeam = x.AwayTeam.Name })
            .ToListAsync();

        return Ok(matches);
    }

    [HttpGet("leagues/{leagueId:guid}/standings")]
    public async Task<IActionResult> GetStandingsByLeagueId(Guid leagueId)
    {
        var team = await
                        _context.Matches.Include(x => x.League).Where(x => x.LeagueId == leagueId).Where(x => x.Status == Status.Finished)
                        .Include(x => x.HomeTeam)
                        .Select(x => new { x.HomeTeam.Name, x.HomeGoals, x.AwayGoals })
                    .Concat(
                        _context.Matches
                        .Include(x => x.AwayTeam).Where(x => x.LeagueId == leagueId)
                        .Select(x => new { x.AwayTeam.Name, HomeGoals = x.AwayGoals, AwayGoals = x.HomeGoals })
                    ).ToListAsync();

        List<Ranking> standings = (from t in team
                                   group t by t.Name into teams
                                   select new Ranking
                                   {
                                       TeamName = teams.Key,
                                       Played = teams.Count(),
                                       Wins = teams.Count(x => x.HomeGoals > x.AwayGoals),
                                       Loses = teams.Count(x => x.HomeGoals < x.AwayGoals),
                                       Draws = teams.Count(x => x.HomeGoals == x.AwayGoals),
                                       GoalsScored = (int)teams.Sum(x => x.HomeGoals),
                                       GoalsConceded = (int)teams.Sum(x => x.AwayGoals),
                                   }).OrderByDescending(x => x.Points)
                                   .ThenByDescending(x => x.GoalsDiff)
                                   .ToList();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return Ok(standings);
    }

    [HttpPost]
    public async Task<IActionResult> AddMatch(Match matchRequest)
    {
        //_context.Matches.Attach(matchRequest);

        await _context.Matches.AddAsync(matchRequest);

        //homeTeam.HomeMatches.Add(match);
        //awayTeam.AwayMatches.Add(match);

        await _context.SaveChangesAsync();

        return Ok(matchRequest);
    }
}
