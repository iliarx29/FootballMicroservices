using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetMatches;
using Matches.Application.Models;
using Matches.Application.Options;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetMatchesQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IElasticService> _elasticMock;
    private readonly IMapper _mapper;

    public GetMatchesQueryHandlerTests()
    {
        _repositoryMock = new();
        _elasticMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetMatches_ShouldExists_ReturnMatches()
    {
        //Arrange
        var options = Options.Create(new ElasticSearchOptions());
        _repositoryMock.Setup(x => x.GetMatchesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Match>());

        _elasticMock.Setup(x => x.CheckIndexExists(It.IsAny<string>())).Returns(true);

        var handler = new GetMatchesQueryHandler(_repositoryMock.Object, _mapper, options, _elasticMock.Object);

        //Act
        var result = await handler.Handle(new(), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<MatchResponse>>();
    }
}
