namespace Teams.Domain.Models;

public class CompetitionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<TeamResponse>? Teams { get; set; }
}
