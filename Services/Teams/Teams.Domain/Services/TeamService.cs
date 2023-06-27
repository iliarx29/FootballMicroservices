using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Shared.RabbitMQ;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Domain.Results;
using Teams.Infrastructure;
using Teams.Infrastructure.Entities;
using Teams.Infrastructure.Repositories.Interfaces;

namespace Teams.Domain.Services;
public class TeamService : ITeamService
{
    private readonly ITeamsRepository _teamsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<TeamRequest> _validator;
    private readonly IEventBus _eventBus;

    public TeamService(ITeamsRepository teamsRepository, IMapper mapper, IValidator<TeamRequest> validator, IEventBus eventBus, IUnitOfWork unitOfWork)
    {
        _teamsRepository = teamsRepository;
        _mapper = mapper;
        _validator = validator;
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<TeamResponse>>> GetAllTeamsAsync()
    {
        var teams = await _teamsRepository.GetAllTeamsAsync();

        var teamsResponse = _mapper.Map<List<TeamResponse>>(teams);

        return Result<List<TeamResponse>>.Success(teamsResponse);
    }

    public async Task<Result<TeamResponse>> GetTeamByIdAsync(Guid id)
    {
        var team = await _teamsRepository.GetTeamByIdAsync(id);

        if (team is null)
            return Result<TeamResponse>.Error(ErrorCode.NotFound, $"Team with id: '{id}' doesn't exists.");

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return Result<TeamResponse>.Success(teamResponse);
    }

    public async Task<Result<TeamResponse>> AddTeamAsync(TeamRequest teamRequest)
    {
        _validator.ValidateAndThrow(teamRequest);

        var team = _mapper.Map<Team>(teamRequest);

        await _teamsRepository.AddTeamAsync(team);
        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync(new TeamCreatedEvent
        {
            Id = team.Id,
            Name = team.Name
        });

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return Result<TeamResponse>.Success(teamResponse);
    }

    public async Task<Result> UpdateTeamAsync(Guid id, TeamRequest teamRequest)
    {
        _validator.ValidateAndThrow(teamRequest);

        var team = await _teamsRepository.GetTeamByIdAsync(id);

        if (team is null)
        {
            return Result.Error(ErrorCode.NotFound, $"Team with given id:'{id}' doesn't exist.");
        }

        team = _mapper.Map<Team>(teamRequest);
        team.Id = id;

        _teamsRepository.UpdateTeam(team);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteTeamAsync(Guid id)
    {
        var team = await _teamsRepository.GetTeamByIdAsync(id);

        if (team is null)
        {
            return Result.Error(ErrorCode.NotFound, $"Team with given id:'{id}' doesn't exist.");
        }

        _teamsRepository.DeleteTeam(team);
        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync(new TeamDeletedEvent(id));

        return Result.Success();
    }

    public async Task<Result<int>> ImportTeams()
    {
        var path = @"C:\Users\iliaa\OneDrive\Рабочий стол\Teams.xlsx";

        using var stream = File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        var teamsCount = await _teamsRepository.GetCount();

        var numbOfMatchesAdded = 0;
        List<Team> teams = new();

        for (int nRow = teamsCount + 2; nRow <= nEndRow; nRow++)
        {
            var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

            var name = row[nRow, 1].GetValue<string>();
            var code = row[nRow, 2].GetValue<string>();
            var countryName = row[nRow, 3].GetValue<string>();
            var city = row[nRow, 4].GetValue<string>();
            var emblem = row[nRow, 5].GetValue<string>();
            var stadium = row[nRow, 6].GetValue<string>();

            var team = new Team
            {
                Name = name,
                Code = code,
                CountryName = countryName,
                City = city,
                Emblem = "",
                Stadium = stadium,
            };

            teams.Add(team);
            numbOfMatchesAdded++;
        }

        await _teamsRepository.AddRange(teams);

        await _unitOfWork.SaveChangesAsync();

        TeamsImportedEvent createdTeams = new(teams.Select(x => new TeamCreatedEvent { Id = x.Id, Name = x.Name }).ToList());

        await _eventBus.PublishAsync(createdTeams);

        return Result<int>.Success(numbOfMatchesAdded);
    }
}
