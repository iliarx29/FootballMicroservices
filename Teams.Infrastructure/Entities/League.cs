namespace Teams.Infrastructure.Entities;

public class League
{
    public Guid Id { get; set; } //PK
    public string Name { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<Team>? Teams { get; set; }
}
