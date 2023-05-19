using Teams.Domain.Models;

namespace Teams.Domain.Interfaces;
public interface ICompetitionService
{
    Task<IEnumerable<CompetitionResponse>> GetAllLeaguesAsync();
    Task<CompetitionResponse> GetLeagueByIdAsync(Guid id);
    Task<CompetitionResponse> AddCompetitionAsync(CompetitionRequest leagueRequest);
    Task UpdateLeagueAsync(Guid id, CompetitionRequest leagueRequest);
    Task DeleteLeagueAsync(Guid id);
    Task<CompetitionResponse> GetCompetitionWithTeams(Guid id);

    Task<int> AddTeamsToCompetition(Guid competitionId, List<Guid> teamIds);
}
