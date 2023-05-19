namespace Teams.Infrastructure.Entities;

public class Competition
{
    public Guid Id { get; set; } //PK
    public string Name { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public CompetitionType Type { get; set; }
    public List<Team>? Teams { get; set; }
}
