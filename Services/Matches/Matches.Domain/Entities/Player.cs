namespace Matches.Domain.Entities;
public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public Guid? TeamId { get; set; }
    public int? ShirtNumber { get; set; }
    public List<Match>? HomeMatches { get; set; } = new();
    public List<Match>? AwayMatches { get; set; } = new();
    public IEnumerable<Match>? AllMatches => HomeMatches?.Concat(AwayMatches).Distinct().OrderByDescending(x => x.MatchDate);
    public DateTime? DateOfBirth { get; set; }
    public Position Position { get; set; }
}
