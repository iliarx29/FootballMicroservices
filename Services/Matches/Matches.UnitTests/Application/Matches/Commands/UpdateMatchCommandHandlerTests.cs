using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Commands.UpdateMatch;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Commands;
public class UpdateMatchCommandHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IElasticService> _elasticMock;
    private readonly IMapper _mapper;

    public UpdateMatchCommandHandlerTests()
    {
        _repositoryMock = new();
        _unitOfWorkMock = new();
        _elasticMock = new(); 

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async void UpdateMatch_Valid_ReturnSuccess()
    {
        //Arrange
        var updateCommand = TestsUtils.UpdateMatchCommand();
        var match = new Match { Id = updateCommand.Id };

        _repositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        _repositoryMock.Setup(x => x.UpdateMatch(match));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

        var handler = new UpdateMatchCommandHandler(_repositoryMock.Object, _mapper, _elasticMock.Object, _unitOfWorkMock.Object);

        //Act
        var result = await handler.Handle(updateCommand, It.IsAny<CancellationToken>());

        //Assert
        _repositoryMock.Verify(x => x.UpdateMatch(It.IsAny<Match>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async void UpdateMatch_Invalid_ReturnError()
    {
        //Arrange
        var updateCommand = TestsUtils.UpdateMatchCommand();

        _repositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var handler = new UpdateMatchCommandHandler(_repositoryMock.Object, _mapper, _elasticMock.Object, _unitOfWorkMock.Object);

        //Act
        var result = await handler.Handle(updateCommand, It.IsAny<CancellationToken>());

        //Assert
        _repositoryMock.Verify(x => x.UpdateMatch(It.IsAny<Match>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        result.IsSuccess.Should().BeFalse();
    }
}
