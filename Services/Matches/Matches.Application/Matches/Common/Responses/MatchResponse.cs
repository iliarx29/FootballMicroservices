using Matches.Domain.Entities;

namespace Matches.Application.Matches.Common.Responses;
public class MatchResponse
{
    public Guid Id { get; set; }
    public Guid HomeTeamId { get; set; }
    public required string HomeTeamName { get; set; }

    public Guid AwayTeamId { get; set; }
    public required string AwayTeamName { get; set; }

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public DateTime? MatchDate { get; set; }

    public Guid CompetitionId { get; set; }
    public required string Season { get; set; }
    public required string Status { get; set; }
    public int? Matchday { get; set; }
    public required string Stage { get; set; }
    public string? Group { get; set; }
    public List<Player> HomePlayers { get; set; } = new();
    public List<Player> AwayPlayers { get; set; } = new();
}
