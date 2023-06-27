using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Models;

namespace Matches.UnitTests;
public class TestsUtils
{
    public static MatchResponse CreateMatchResponse()
    {
        var matchResponse = new MatchResponse()
        {
            Id = Guid.NewGuid(),
            HomeTeamId = Guid.NewGuid(),
            AwayTeamId = Guid.NewGuid(),
            CompetitionId = Guid.NewGuid(),
            HomeTeamName = "homeTeam",
            AwayTeamName = "awayTeam",
            HomeGoals = null,
            AwayGoals = null,
            Season = "2022/2023",
            Status = "Scheduled",
            Stage = "Regular_Season",
            Group = null,
            Matchday = 1,
            MatchDate = DateTime.UtcNow
        };

        return matchResponse;
    }

    public static CreateMatchCommand CreateMatchCommand()
    {
        var command = new CreateMatchCommand(
            HomeTeamId: Guid.NewGuid(),
            AwayTeamId: Guid.NewGuid(),
            HomeGoals: null,
            AwayGoals: null,
            MatchDate: DateTime.Now,
            CompetitionId: Guid.NewGuid(),
            Season: "2022/2023",
            Status: "Scheduled",
            Matchday: 4,
            Group: null,
            Stage: "Regular_Season",
            HomePlayers: new(),
            AwayPlayers: new());

        return command;
    }

    public static UpdateMatchCommand UpdateMatchCommand()
    {
        var command = new UpdateMatchCommand(
            Id: Guid.NewGuid(),
            HomeTeamId: Guid.NewGuid(),
            AwayTeamId: Guid.NewGuid(),
            HomeGoals: null,
            AwayGoals: null,
            MatchDate: DateTime.Now,
            CompetitionId: Guid.NewGuid(),
            Season: "2022/2023",
            Status: "Scheduled",
            Matchday: 4,
            Group: null,
            Stage: "Regular_Season");

        return command;
    }
}
