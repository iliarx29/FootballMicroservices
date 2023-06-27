using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
using Matches.Application.Models;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetMatchesByCompetitionIdQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetMatchesByCompetitionIdQueryHandlerTests()
    {
        _repositoryMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetMatchesByCompetitionId_ValidRequest_ReturnMatches()
    {
        //Arrange
        _repositoryMock.Setup(x => x.GetMatchesByCompetitionId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Match>());

        var handler = new GetMatchesByCompetitionIdQueryHandler(_repositoryMock.Object, _mapper);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<List<MatchResponse>>();
    }
}
