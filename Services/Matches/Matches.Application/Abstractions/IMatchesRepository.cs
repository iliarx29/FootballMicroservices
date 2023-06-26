using Matches.Domain.Entities;

namespace Matches.Application.Abstractions;
public interface IMatchesRepository
{
    Task<List<Match>> GetMatchesAsync(CancellationToken cancellationToken = default);
    Task<Match?> GetMatchByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Match>> GetH2HMatchesAsync(Guid homeTeamId, Guid awayTeamId, CancellationToken cancellationToken = default);
    Task<List<Match>> GetMatchesByCompetitionId(Guid id, CancellationToken cancellationToken = default);
    Task<List<Match>> GetMatchesByTeamId(Guid id, CancellationToken cancellationToken = default);
    List<Ranking> GetStandingsByCompetitionAndSeason(Guid competitionId, string season);
    void AddMatch(Match match);
    void UpdateMatch(Match match);
    void DeleteMatch(Match match);

}
