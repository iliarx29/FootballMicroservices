using Teams.Domain.Models;
using Teams.Domain.Results;

namespace Teams.Domain.Interfaces;
public interface ITeamService
{
    Task<Result<IEnumerable<TeamResponse>>> GetAllTeamsAsync();
    Task<Result<TeamResponse>> GetTeamByIdAsync(Guid id);

    Task<Result<TeamResponse>> AddTeamAsync(TeamRequest teamRequest);
    Task<Result> UpdateTeamAsync(Guid id, TeamRequest teamRequest);
    Task<Result> DeleteTeamAsync(Guid id);
    Task<Result<int>> ImportTeams();
}
