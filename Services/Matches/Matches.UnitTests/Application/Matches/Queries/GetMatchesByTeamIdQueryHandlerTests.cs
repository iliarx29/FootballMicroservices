using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetMatchesByTeamId;
using Matches.Application.Models;
using Moq;
using System.Text.Json;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetMatchesByTeamIdQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IRedisService> _redisMock;
    private readonly IMapper _mapper;

    public GetMatchesByTeamIdQueryHandlerTests()
    {
        _repositoryMock = new();
        _redisMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetMatchesByTeamId_FromDB_ReturnMatches()
    {
        //Arrange
        var strValue = It.IsAny<string>();
        var ct = It.IsAny<CancellationToken>();

        _redisMock.Setup(x => x.GetStringAsync(strValue, ct)).ReturnsAsync(string.Empty);

        _repositoryMock.Setup(x => x.GetMatchesByTeamId(It.IsAny<Guid>(), ct))
            .ReturnsAsync(new List<Match>());

        _redisMock.Setup(x => x.SetStringAsync(strValue, strValue, new(), ct));

        var handler = new GetMatchesByTeamIdQueryHandler(_repositoryMock.Object, _redisMock.Object, _mapper);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), ct);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<List<MatchResponse>>();
    }

    [Fact]
    public async Task GetMatchesByTeamId_FromCache_ReturnMatches()
    {
        //Arrange
        var matchResponse = TestsUtils.CreateMatchResponse();
        var cacheMatches = JsonSerializer.Serialize(new List<MatchResponse>() { matchResponse });
        var key = $"matchesByTeamId-{It.IsAny<Guid>()}";
        var ct = It.IsAny<CancellationToken>();

        _redisMock.Setup(x => x.GetStringAsync(key, ct)).ReturnsAsync(cacheMatches);

        var handler = new GetMatchesByTeamIdQueryHandler(_repositoryMock.Object, _redisMock.Object, _mapper);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), ct);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<List<MatchResponse>>();
    }
}
