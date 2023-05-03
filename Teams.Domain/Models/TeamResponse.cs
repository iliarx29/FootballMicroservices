namespace Teams.Domain.Models;

public class TeamResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Emblem { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public Guid LeagueId { get; set; }
}
