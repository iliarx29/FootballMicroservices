using Teams.Domain.Models;

namespace Teams.Domain.Interfaces;
public interface ILeagueService
{
    Task<IEnumerable<LeagueResponse>> GetAllLeaguesAsync();
    Task<LeagueResponse> GetLeagueByIdAsync(Guid id);
    Task<LeagueResponse> AddLeagueAsync(LeagueRequest leagueRequest);
    Task UpdateLeagueAsync(Guid id, LeagueRequest leagueRequest);
    Task DeleteLeagueAsync(Guid id);
    Task<LeagueResponse> GetTeamsOfLeagueAsync(Guid id);
}
