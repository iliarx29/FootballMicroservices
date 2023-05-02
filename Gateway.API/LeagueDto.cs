namespace Gateway.API;

public class LeagueDto
{
    public Guid Id { get; set; } //PK
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public List<TeamDto>? Teams {  get; set; } 
}
