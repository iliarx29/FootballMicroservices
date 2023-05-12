using Teams.Domain.Models;
namespace Teams.Domain.Interfaces;
public interface ITeamService
{
    Task<IEnumerable<TeamResponse>> GetAllTeamsAsync();
    Task<TeamResponse> GetTeamByIdAsync(Guid id);

    Task<TeamResponse> AddTeamAsync(TeamRequest teamRequest);
    Task UpdateTeamAsync(Guid id, TeamRequest teamRequest);
    Task DeleteTeamAsync(Guid id);
    Task<int> ImportTeams();
}
