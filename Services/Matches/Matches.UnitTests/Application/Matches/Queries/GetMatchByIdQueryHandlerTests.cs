using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetMatchById;
using Matches.Application.Models;
using Matches.Application.Results;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Queries;
public class GetMatchByIdQueryHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public GetMatchByIdQueryHandlerTests()
    {
        _repositoryMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async void GetMatchById_ValidMatchId_ReturnMatch()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var exceptedMatch = new Match() { Id = validId };

        _repositoryMock.Setup(x => x.GetMatchByIdAsync(validId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exceptedMatch);

        var queryHandler = new GetMatchByIdQueryHandler(_mapper, _repositoryMock.Object);

        //Act
        var result = await queryHandler.Handle(new GetMatchByIdQuery(validId), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MatchResponse>();
        result.Value.Id.Should().Be(exceptedMatch.Id);
    }

    [Fact]
    public async void GetMatchById_InvalidMatchId_ReturnError()
    {
        //Arrange
        var invalidId = Guid.NewGuid();
        _repositoryMock.Setup(x => x.GetMatchByIdAsync(invalidId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var queryHandler = new GetMatchByIdQueryHandler(_mapper, _repositoryMock.Object);

        //Act
        var result = await queryHandler.Handle(new GetMatchByIdQuery(invalidId), It.IsAny<CancellationToken>());

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorCode.Should().NotBeNull().And.BeOfType<ErrorCode>();
    }

}
