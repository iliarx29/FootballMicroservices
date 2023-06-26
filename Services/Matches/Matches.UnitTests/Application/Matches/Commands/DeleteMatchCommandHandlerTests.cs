using AutoMapper;
using FluentAssertions;
using Matches.Application.Abstractions;
using Matches.Application.Mappings;
using Matches.Application.Matches.Commands.DeleteMatch;
using Moq;
using Match = Matches.Domain.Entities.Match;

namespace Matches.UnitTests.Application.Matches.Commands;
public class DeleteMatchCommandHandlerTests
{
    private readonly Mock<IMatchesRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRedisService> _redisMock;
    private readonly IMapper _mapper;

    public DeleteMatchCommandHandlerTests()
    {
        _repositoryMock = new();
        _unitOfWorkMock = new();
        _redisMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async void DeleteMatch_Valid_ReturnSuccess()
    {
        //Arrange
        var match = new Match();

        _repositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        _repositoryMock.Setup(x => x.DeleteMatch(match));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

        var handler = new DeleteMatchCommandHandler(_repositoryMock.Object, _redisMock.Object, _unitOfWorkMock.Object);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), It.IsAny<CancellationToken>());

        //Assert
        _repositoryMock.Verify(x => x.DeleteMatch(It.IsAny<Match>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async void DeleteMatch_Invalid_ReturnError()
    {
        //Arrange
        _repositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var handler = new DeleteMatchCommandHandler(_repositoryMock.Object, _redisMock.Object, _unitOfWorkMock.Object);

        //Act
        var result = await handler.Handle(new(It.IsAny<Guid>()), It.IsAny<CancellationToken>());

        //Assert
        _repositoryMock.Verify(x => x.DeleteMatch(It.IsAny<Match>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        result.IsSuccess.Should().BeFalse();
    }
}
