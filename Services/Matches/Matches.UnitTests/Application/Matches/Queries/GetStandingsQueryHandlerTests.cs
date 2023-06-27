using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetMatchesByTeamId;
using Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
using Matches.Application.Models;
using Matches.Domain.Entities;
using Moq;
using System.Text.Json;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetStandingsQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IRedisService> _redisMock;
    private readonly string _season = "2022/2023";

    public GetStandingsQueryHandlerTests()
    {
        _repositoryMock = new();
        _redisMock = new();
    }

    [Fact]
    public async Task GetStandings_FromDB_ReturnMatches()
    {
        //Arrange
        var strValue = It.IsAny<string>();
        var ct = It.IsAny<CancellationToken>();

        _redisMock.Setup(x => x.GetStringAsync(strValue, ct)).ReturnsAsync(string.Empty);

        _repositoryMock.Setup(x => x.GetStandingsByCompetitionAndSeason(It.IsAny<Guid>(), _season))
            .Returns(new List<Ranking>());

        _redisMock.Setup(x => x.SetStringAsync(strValue, strValue, new(), ct));

        var handler = new GetStandingsByCompetitionAndSeasonQueryHandler(_repositoryMock.Object, _redisMock.Object);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>(), _season), ct);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<List<Ranking>>();
    }

    [Fact]
    public async Task GetMatchesByTeamId_FromCache_ReturnMatches()
    {
        //Arrange
        var cacheMatches = JsonSerializer.Serialize(new List<MatchResponse>());
        var key = $"standings-{It.IsAny<Guid>()}+{_season}";
        var ct = It.IsAny<CancellationToken>();

        _redisMock.Setup(x => x.GetStringAsync(key, ct)).ReturnsAsync(cacheMatches);

        var handler = new GetStandingsByCompetitionAndSeasonQueryHandler(_repositoryMock.Object, _redisMock.Object);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>(), _season), ct);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<List<Ranking>>();
    }
}
