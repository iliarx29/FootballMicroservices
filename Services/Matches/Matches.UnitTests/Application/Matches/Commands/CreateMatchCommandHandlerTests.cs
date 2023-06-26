using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Models;
using Matches.Application.Results;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Commands;
public class CreateMatchCommandHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRedisService> _redisMock;
    private readonly Mock<IElasticService> _elasticMock;
    private readonly IMapper _mapper;

    public CreateMatchCommandHandlerTests()
    {
        _repositoryMock = new();
        _unitOfWorkMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();

        _elasticMock = new();
        _redisMock = new();
    }

    [Fact]
    public async void CreateMatch_Valid_ReturnCreatedMatch()
    {
        // Arrange
        CreateMatchCommand command = TestsUtils.CreateMatchCommand();

        _repositoryMock.Setup(x => x.AddMatch(It.IsAny<Match>()));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

        _elasticMock.Setup(x => x.AddDocument(It.IsAny<MatchSearchResponse>(), It.IsAny<CancellationToken>()));

        var handler = new CreateMatchCommandHandler(_redisMock.Object, _mapper, _repositoryMock.Object, 
            _unitOfWorkMock.Object, _elasticMock.Object);

        // Act
        Result<Match> result = await handler.Handle(command, It.IsAny<CancellationToken>());

        // Assert
        _repositoryMock.Verify(x => x.AddMatch(It.IsAny<Match>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.BeOfType<Match>();
    }
}
