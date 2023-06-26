using FluentAssertions;
using Moq;
using System.Net;
using Teams.API.Controllers;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Domain.Results;
using Teams.Infrastructure.Entities;

namespace Teams.UnitTests.Controllers;
public class TeamControllerTests
{
    private readonly Mock<ITeamService> _mockService;

    public TeamControllerTests()
    {
        _mockService = new();
    }

    [Fact]
    public async Task GetTeamById_ValidId_ReturnOk()
    {
        //Arrange
        var teamResult = Result<TeamResponse>.Success(new TeamResponse());
        _mockService.Setup(x => x.GetTeamByIdAsync(It.IsAny<Guid>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.GetTeamById(It.IsAny<Guid>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.OK);
        actualResult.ResultBody.Value.Should().BeOfType<TeamResponse>().And.NotBeNull();
    }

    [Fact]
    public async Task GetTeamById_InvalidId_ReturnNotFound()
    {
        //Arrange
        var teamResult = Result<TeamResponse>.Error(ErrorCode.NotFound, "Not found");
        _mockService.Setup(x => x.GetTeamByIdAsync(It.IsAny<Guid>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.GetTeamById(It.IsAny<Guid>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllTeams_TeamsExist_ReturnOk()
    {
        //Arrange
        var teamsResult = Result<List<TeamResponse>>.Success(new List<TeamResponse>());
        _mockService.Setup(x => x.GetAllTeamsAsync()).ReturnsAsync(teamsResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.GetAllTeams();

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.OK);
        actualResult.ResultBody.Value.Should().BeOfType<List<TeamResponse>>().And.NotBeNull();
    }

    [Fact]
    public async Task GetAllTeams_TeamsNotExist_ReturnNotFound()
    {
        //Arrange
        var teamsResult = Result<List<TeamResponse>>.Error(ErrorCode.NotFound, "Not found");
        _mockService.Setup(x => x.GetAllTeamsAsync()).ReturnsAsync(teamsResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.GetAllTeams();

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddTeam_ValidRequest_ReturnOk()
    {
        //Arrange
        var teamResult = Result<TeamResponse>.Success(new TeamResponse());
        _mockService.Setup(x => x.AddTeamAsync(It.IsAny<TeamRequest>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.AddTeam(It.IsAny<TeamRequest>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.Created);
        actualResult.ResultBody.Value.Should().BeOfType<TeamResponse>().And.NotBeNull();
    }

    [Fact]
    public async Task AddTeam_InvalidRequest_ReturnNotFound()
    {
        //Arrange
        var teamResult = Result<TeamResponse>.Error(ErrorCode.NotFound, "Not found");
        _mockService.Setup(x => x.AddTeamAsync(It.IsAny<TeamRequest>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.AddTeam(It.IsAny<TeamRequest>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTeam_ValidRequest_ReturnNoContent()
    {
        //Arrange
        var teamResult = Result.Success();
        _mockService.Setup(x => x.UpdateTeamAsync(It.IsAny<Guid>(), It.IsAny<TeamRequest>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.UpdateTeam(It.IsAny<Guid>(), It.IsAny<TeamRequest>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateTeam_InvalidRequest_ReturnNotFound()
    {
        //Arrange
        var teamResult = Result.Error(ErrorCode.NotFound, "Not found");
        _mockService.Setup(x => x.UpdateTeamAsync(It.IsAny<Guid>(), It.IsAny<TeamRequest>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.UpdateTeam(It.IsAny<Guid>(), It.IsAny<TeamRequest>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTeam_ValidRequest_ReturnNoContent()
    {
        //Arrange
        var teamResult = Result.Success();
        _mockService.Setup(x => x.DeleteTeamAsync(It.IsAny<Guid>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.DeleteTeam(It.IsAny<Guid>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTeam_InvalidRequest_ReturnNotFound()
    {
        //Arrange
        var teamResult = Result.Error(ErrorCode.NotFound, "Not found");
        _mockService.Setup(x => x.DeleteTeamAsync(It.IsAny<Guid>())).ReturnsAsync(teamResult);

        var controller = new TeamsController(_mockService.Object);

        //Act
        var actualResult = await controller.DeleteTeam(It.IsAny<Guid>());

        //Assert
        actualResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
