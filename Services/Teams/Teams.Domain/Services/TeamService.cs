using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Teams.Domain.Exceptions;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Domain.Validation;
using Teams.Infrastructure;
using Teams.Infrastructure.Entities;

namespace Teams.Domain.Services;
public class TeamService : ITeamService
{
    private readonly TeamsDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<TeamRequest> _validator;

    public TeamService(TeamsDbContext context, IMapper mapper, IValidator<TeamRequest> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<TeamResponse>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams.AsNoTracking().ToListAsync();

        var teamsResponse = _mapper.Map<List<TeamResponse>>(teams);

        return teamsResponse;
    }

    public async Task<TeamResponse> GetTeamByIdAsync(Guid id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (team is null)
            throw new NotFoundException($"Team with id: '{id}' doesn't exists.");

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return teamResponse;
    }

    public async Task<TeamResponse> AddTeamAsync(TeamRequest teamRequest)
    {
        _validator.ValidateAndThrow(teamRequest);

        var team = _mapper.Map<Team>(teamRequest);

        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return teamResponse;
    }

    public async Task UpdateTeamAsync(Guid id, TeamRequest teamRequest)
    {
        _validator.ValidateAndThrow(teamRequest);

        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (team is null)
        {
            throw new NotFoundException($"Team with given id:'{id}' doesn't exist.");
        }

        team = _mapper.Map<Team>(teamRequest);
        team.Id = id;

        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
        if (team is null)
        {
            throw new NotFoundException($"Team with given id:'{id}' doesn't exist.");
        }

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ImportTeams()
    {
        var path = @"C:\Users\Ilya\Desktop\Teams.xlsx";

        using var stream = System.IO.File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        var numbOfMatchesAdded = 0;
        List<Team> teams = new();

        for (int nRow = 2; nRow <= 25; nRow++)
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

        await _context.AddRangeAsync(teams);

        await _context.SaveChangesAsync();

        return numbOfMatchesAdded;
    }
}
