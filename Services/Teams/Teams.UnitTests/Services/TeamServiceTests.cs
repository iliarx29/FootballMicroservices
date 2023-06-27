using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Shared.RabbitMQ;
using Teams.Domain.Interfaces;
using Teams.Domain.Mappings;
using Teams.Domain.Models;
using Teams.Domain.Services;
using Teams.Infrastructure.Entities;
using Teams.Infrastructure.Repositories.Interfaces;
using Xunit.Sdk;

namespace Teams.UnitTests.Services;
public class TeamServiceTests
{
    private readonly Mock<ITeamsRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<IValidator<TeamRequest>> _validatorMock;
    private readonly IMapper _mapper;

    public TeamServiceTests()
    {
        _repositoryMock = new();
        _unitOfWorkMock = new();
        _eventBusMock = new();
        _validatorMock = new();

        var mockMapper = new MapperConfiguration(cfg =>
            cfg.AddProfile(new MappingProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetTeams_Should_ReturnListOfTeamResponse()
    {
        //Arrange
        var teams = new List<Team>();
        _repositoryMock.Setup(x => x.GetAllTeamsAsync()).ReturnsAsync(teams);

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        //Act
        var actualResult = await teamService.GetAllTeamsAsync();

        //Assert
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value.Should().NotBeNull();
        actualResult.Value.Should().BeOfType<List<TeamResponse>>();
    }

    [Fact]
    public async Task GetTeamById_ValidTeamId_ReturnTeamResponse()
    {
        //Arrange
        var teamId = Guid.NewGuid();
        var team = new Team() { Id = teamId };
        _repositoryMock.Setup(x => x.GetTeamByIdAsync(teamId)).ReturnsAsync(team);

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        //Act
        var actualResult = await teamService.GetTeamByIdAsync(teamId);

        //Assert
        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value.Should().NotBeNull();
        actualResult.Value.Should().BeOfType<TeamResponse>();
        actualResult.Value?.Id.Should().Be(teamId);
    }

    [Fact]
    public async Task GetTeamById_InvalidTeamId_ReturnError()
    {
        //Arrange
        _repositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        //Act
        var actualResult = await teamService.GetTeamByIdAsync(It.IsAny<Guid>());

        //Assert
        actualResult.IsSuccess.Should().BeFalse();
        actualResult.ErrorCode.Should().NotBeNull();
        actualResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AddTeam_Should_ReturnCreatedTeam()
    {
        var newTeam = new Team();
        var teamRequest = new TeamRequest();

        _repositoryMock.Setup(x => x.AddTeamAsync(newTeam));
        _eventBusMock.Setup(x => x.PublishAsync(It.IsAny<TeamCreatedEvent>(), It.IsAny<CancellationToken>()));

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        var actualResult = await teamService.AddTeamAsync(teamRequest);

        _repositoryMock.Verify(x => x.AddTeamAsync(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        actualResult.IsSuccess.Should().BeTrue();
        actualResult.Value.Should().NotBeNull();
        actualResult.Value.Should().BeOfType<TeamResponse>();
    }

    [Fact]
    public async Task UpdateTeam_ValidId_ReturnSuccess()
    {
        var teamId = Guid.NewGuid();
        var team = new Team() { Id = teamId };

        var teamRequest = new TeamRequest();

        _repositoryMock.Setup(x => x.GetTeamByIdAsync(teamId)).ReturnsAsync(team);

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        var actualResult = await teamService.UpdateTeamAsync(teamId, teamRequest);

        _repositoryMock.Verify(x => x.UpdateTeam(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        actualResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTeam_InvalidId_ReturnError()
    {
        var teamId = Guid.NewGuid();
        var team = new Team() { Id = teamId };

        var teamRequest = new TeamRequest();

        _repositoryMock.Setup(x => x.GetTeamByIdAsync(teamId)).ReturnsAsync(() => null);

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        var actualResult = await teamService.UpdateTeamAsync(teamId, teamRequest);

        _repositoryMock.Verify(x => x.UpdateTeam(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        actualResult.IsSuccess.Should().BeFalse();
        actualResult.ErrorCode.Should().NotBeNull();
        actualResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteTeam_ValidId_ReturnSuccess()
    {
        var teamId = Guid.NewGuid();
        var team = new Team() { Id = teamId };

        var teamRequest = new TeamRequest();

        _repositoryMock.Setup(x => x.GetTeamByIdAsync(teamId)).ReturnsAsync(team);
        _eventBusMock.Setup(x => x.PublishAsync(It.IsAny<TeamDeletedEvent>(), It.IsAny<CancellationToken>()));

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        var actualResult = await teamService.DeleteTeamAsync(teamId);

        _repositoryMock.Verify(x => x.DeleteTeam(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        actualResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTeam_InvalidId_ReturnError()
    {
        var teamId = Guid.NewGuid();
        var team = new Team() { Id = teamId };

        var teamRequest = new TeamRequest();

        _repositoryMock.Setup(x => x.GetTeamByIdAsync(teamId)).ReturnsAsync(() => null);
        _eventBusMock.Setup(x => x.PublishAsync(It.IsAny<TeamDeletedEvent>(), It.IsAny<CancellationToken>()));

        var teamService = new TeamService(_repositoryMock.Object, _mapper, _validatorMock.Object, _eventBusMock.Object, _unitOfWorkMock.Object);

        var actualResult = await teamService.DeleteTeamAsync(teamId);

        _repositoryMock.Verify(x => x.DeleteTeam(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        actualResult.IsSuccess.Should().BeFalse();
        actualResult.ErrorCode.Should().NotBeNull();
        actualResult.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
