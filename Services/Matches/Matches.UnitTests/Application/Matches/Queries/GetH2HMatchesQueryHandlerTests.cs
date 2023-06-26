using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetH2HMatches;
using Matches.Application.Models;
using Matches.Application.Results;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetH2HMatchesQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetH2HMatchesQueryHandlerTests()
    {
        _repositoryMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetH2HMatches_ValidRequest_ReturnMatches()
    {
        //Arrange
        var firstTeam = Guid.NewGuid();
        var secondTeam = Guid.NewGuid();

        var validH2HMatches = new List<Match>()
        {
            new Match() {HomeTeamId = firstTeam, AwayTeamId = secondTeam},
            new Match() {HomeTeamId = secondTeam, AwayTeamId = firstTeam},
            new Match() {HomeTeamId = firstTeam, AwayTeamId = secondTeam},
            new Match() {HomeTeamId = firstTeam, AwayTeamId = secondTeam},
            new Match() {HomeTeamId = secondTeam, AwayTeamId = firstTeam}
        };

        Guid existingMatchId = Guid.NewGuid();
        var existingMatch = new Match() { Id = existingMatchId, HomeTeamId = firstTeam, AwayTeamId = secondTeam };
        _repositoryMock.Setup(x => x.GetMatchByIdAsync(existingMatchId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingMatch);

        _repositoryMock.Setup(x => x.GetH2HMatchesAsync(existingMatch.HomeTeamId, existingMatch.AwayTeamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validH2HMatches);

        var handler = new GetH2HMatchesQueryHandler(_repositoryMock.Object, _mapper);

        //Act
        var result = await handler.Handle(new(existingMatchId), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<MatchResponse>>();
        result.Value.Should()
            .OnlyContain(x => x.HomeTeamId == existingMatch.HomeTeamId || x.HomeTeamId == existingMatch.AwayTeamId)
            .And
            .OnlyContain(x => x.AwayTeamId == existingMatch.HomeTeamId || x.AwayTeamId == existingMatch.AwayTeamId);
    }

    [Fact]
    public async Task GetH2HMatches_InvalidRequest_ReturnMatches()
    {
        //Arrange
        _repositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var handler = new GetH2HMatchesQueryHandler(_repositoryMock.Object, _mapper);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().NotBeNull().And.BeOfType<ErrorCode>();
    }
}
