namespace Teams.Domain.Models;

public class TeamRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Emblem { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
}
