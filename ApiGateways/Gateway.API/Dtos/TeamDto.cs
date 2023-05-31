namespace Gateway.API.Dtos;

public class TeamDto
{
    public Guid Id { get; set; }  //PK
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public string City { get; set; } = string.Empty;
    public string Emblem { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public Guid LeagueId { get; set; }
}
