namespace Matches.Domain.Entities;

public class Ranking
{
    public int Position { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid TeamId { get; set; }
    public int Played { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Loses { get; set; }
    public int GoalsScored { get; set; }
    public int GoalsConceded { get; set; }
    public int Points => Wins * 3 + Draws;
    public int GoalsDiff => GoalsScored - GoalsConceded;
}
