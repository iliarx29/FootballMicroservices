using System.Numerics;

namespace Matches.Domain.Entities;

public class Match
{
    public Guid Id { get; set; } //PK
    public Guid HomeTeamId { get; set; } //FK
    //public Team? HomeTeam { get; set; } // Reference navigation

    public Guid AwayTeamId { get; set; } // FK
    //public Team? AwayTeam { get; set; } // Reference navigation

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public DateTime? MatchDate { get; set; }

    public Guid LeagueId { get; set; } //FK
    //public League? League { get; set; } // Reference navigation
     public Guid SeasonId { get; set; } //FK
    public Season Season { get; set; }
    public Status Status { get; set; }
    public int Round { get; set; }
}
