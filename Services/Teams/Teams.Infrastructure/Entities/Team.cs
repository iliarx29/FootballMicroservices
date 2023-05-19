namespace Teams.Infrastructure.Entities;

public class Team
{
    public Guid Id { get; set; }  //PK
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Emblem { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public List<Competition> Competitions { get; set; } = new();
}
