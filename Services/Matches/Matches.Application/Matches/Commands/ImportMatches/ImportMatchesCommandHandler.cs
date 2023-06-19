using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Matches.Application.Matches.Commands.ImportMatches;
public class ImportMatchesCommandHandler : IRequestHandler<ImportMatchesCommand, Result<int>>
{
    private readonly IMatchesDbContext _context;

    public ImportMatchesCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(ImportMatchesCommand request, CancellationToken cancellationToken)
    {
        if (request.File.Length > 0)
        {
            var teams = await _context.Teams.ToListAsync(cancellationToken);

            var teamsDict = new Dictionary<string, Guid>();

            foreach (var item in teams)
            {
                teamsDict.Add(item.Name, item.Id);
            }

            var result = await ImportMatches(teamsDict, request.CompetitionId, request.Season, request.File);

            return Result<int>.Success(result);
        }

        return Result<int>.Error(ErrorCode.NotFound, "File not found");
    }

    private async Task<int> ImportMatches(Dictionary<string, Guid> teamsDict, Guid competitionId, string season, IFormFile file)
    {
        using var stream = File.Create(file.FileName);
        file.CopyTo(stream);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        var matchesCount = await _context.Matches.CountAsync(x => x.CompetitionId == competitionId && x.Season == season);

        var numbOfMatchesAdded = 0;
        List<Match> matches = new();

        for (int nRow = matchesCount + 2; nRow <= nEndRow; nRow++)
        {
            var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

            var homeTeamName = row[nRow, 4].GetValue<string>();
            var awayTeamName = row[nRow, 5].GetValue<string>();
            var homeGoals = row[nRow, 6].GetValue<int>();
            var awayGoals = row[nRow, 7].GetValue<int>();
            var date = row[nRow, 2].GetValue<DateTime>();
            var time = row[nRow, 3].GetValue<TimeSpan>();

            teamsDict.TryGetValue(homeTeamName, out Guid homeTeamId);
            teamsDict.TryGetValue(awayTeamName, out Guid awayTeamId);

            var match = new Match
            {
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                HomeGoals = homeGoals,
                AwayGoals = awayGoals,
                CompetitionId = competitionId,
                Season = season,
                MatchDate = DateTime.SpecifyKind(date.Add(TimeSpan.Parse(time.ToString())), DateTimeKind.Utc),
                Status = Status.Finished,
                Stage = Stage.Regular_Season,
            };

            matches.Add(match);
            numbOfMatchesAdded++;
        }

        await _context.Matches.AddRangeAsync(matches);

        await _context.SaveChangesAsync();

        return numbOfMatchesAdded;
    }
}
