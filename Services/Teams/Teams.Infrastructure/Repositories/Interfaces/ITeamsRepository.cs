using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure.Repositories.Interfaces;
public interface ITeamsRepository
{
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(Guid id);

    Task AddTeamAsync(Team team);
    Task AddRange(List<Team> teams);
    void UpdateTeam(Team team);
    void DeleteTeam(Team team);
    Task<int> ImportTeams();

    Task<int> GetCount();
}
