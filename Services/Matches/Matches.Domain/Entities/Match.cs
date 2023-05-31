using Matches.Domain.Entities.Enums;

namespace Matches.Domain.Entities;

public class Match
{
    public Guid Id { get; set; } //PK
    public Guid HomeTeamId { get; set; } //FK
    public Team HomeTeam { get; set; }

    public Guid AwayTeamId { get; set; } // FK
    public Team AwayTeam { get; set; }

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public DateTime? MatchDate { get; set; }

    public Guid CompetitionId { get; set; } //FK
    public string Season { get; set; } = string.Empty;
    public Status Status { get; set; }
    public int? Matchday { get; set; }
    public Stage Stage { get; set; }
    public Group? Group { get; set; }
    public List<Player> HomePlayers { get; set; } = new();
    public List<Player> AwayPlayers { get; set; } = new();
}
