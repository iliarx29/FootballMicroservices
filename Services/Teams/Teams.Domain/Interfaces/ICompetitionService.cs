using Teams.Domain.Models;
using Teams.Domain.Results;

namespace Teams.Domain.Interfaces;
public interface ICompetitionService
{
    Task<Result<IEnumerable<CompetitionResponse>>> GetAllCompetitionsAsync();
    Task<Result<CompetitionResponse>> GetCompetitionByIdAsync(Guid id);
    Task<Result<CompetitionResponse>> AddCompetitionAsync(CompetitionRequest competitionRequest);
    Task<Result> UpdateCompetitionAsync(Guid id, CompetitionRequest competitionRequest);
    Task<Result> DeleteCompetitionAsync(Guid id);
    Task<Result<CompetitionResponse>> GetCompetitionWithTeams(Guid id);

    Task<Result<int>> AddTeamsToCompetition(Guid competitionId, List<Guid> teamsIds);
    Task<Result<int>> RemoveTeamsFromCompetition(Guid competitionId, List<Guid> teamsIds);
}
