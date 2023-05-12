namespace Matches.Application.Matches.Commands.ImportMatches.Models;
internal class LeagueResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public List<TeamResponse>? Teams { get; set; }
}
