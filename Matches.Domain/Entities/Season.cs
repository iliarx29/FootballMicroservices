namespace Matches.Domain.Entities;

public class Season
{
    public Guid Id { get; set; }
    public string Years { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? TeamWinnerId { get; set; }
    public Guid LeagueId { get; set; }
}