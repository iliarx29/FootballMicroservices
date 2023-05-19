using Teams.Domain.Models;

namespace Teams.Domain.Interfaces;
public interface ICompetitionService
{
    Task<IEnumerable<CompetitionResponse>> GetAllCompetitionsAsync();
    Task<CompetitionResponse> GetCompetitionByIdAsync(Guid id);
    Task<CompetitionResponse> AddCompetitionAsync(CompetitionRequest competitionRequest);
    Task UpdateCompetitionAsync(Guid id, CompetitionRequest competitionRequest);
    Task DeleteCompetitionAsync(Guid id);
    Task<CompetitionResponse> GetCompetitionWithTeams(Guid id);

    Task<int> AddTeamsToCompetition(Guid competitionId, List<Guid> teamIds);
}
