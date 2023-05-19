namespace Teams.Infrastructure.Entities;

public class Team
{
    public Guid Id { get; set; }  //PK
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Emblem { get; set; } = string.Empty;
    public string Stadium { get; set; } = null!;
    public List<Competition> Competitions { get; set; }
}
