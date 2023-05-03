using System.Numerics;

namespace Matches.API.Entities;

public class Match
{
    public Guid Id { get; set; } //PK
    public Guid HomeTeamId { get; set; } //FK
    public Team? HomeTeam { get; set; } // Reference navigation

    public Guid AwayTeamId { get; set; } // FK
    public Team? AwayTeam { get; set; } // Reference navigation

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    //public List<Player> HomePlayers { get; set; } = new();
    //public List<Player> AwayPlayers { get; set; } = new();
    public DateTime MatchDate { get; set; }

    public Guid LeagueId { get; set; } //FK
    public League? League { get; set; } // Reference navigation
    public string Season { get; set; } = string.Empty;
    public Status Status { get; set; }
}

public enum Status
{
    Scheduled,
    Live,
    Finished,
    Postponed
}
