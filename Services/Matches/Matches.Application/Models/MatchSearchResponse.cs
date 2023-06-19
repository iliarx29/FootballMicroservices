namespace Matches.Application.Models;

public class MatchSearchResponse
{
    public Guid Id { get; set; }
    public required string HomeTeamName { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public required string AwayTeamName { get; set; }
    public int? Matchday { get; set; }
    public required string Season { get; set; }
    public DateTime MatchDate { get; set; }
    public required string Status { get; set; }
    public Guid CompetitionId { get; set; }

}