using Matches.Application.Abstractions;
using Matches.Application.Matches.Commands.ImportMatches.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using OfficeOpenXml;
using System.Net.Http.Json;

namespace Matches.Application.Matches.Commands.ImportMatches;
public class ImportMatchesCommandHandler : IRequestHandler<ImportMatchesCommand, Result<int>>
{
    private readonly IHttpClientFactory _client;
    private readonly IMatchesDbContext _context;

    public ImportMatchesCommandHandler(IHttpClientFactory client, IMatchesDbContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<Result<int>> Handle(ImportMatchesCommand request, CancellationToken cancellationToken)
    {
        //await _context.Matches.ExecuteDeleteAsync();

        var httpClient = _client.CreateClient();
        var url = $"https://localhost:7057/api/competitions/{request.CompetitionId}/teams";

        var response = await httpClient.GetFromJsonAsync<CompetitionResponse>(url);

        if (response is null)
            return Result<int>.Error(ErrorCode.NotFound, $"League with id: '{command.LeagueId}' not found");

        if (response.Teams is null)
            return Result<int>.Error(ErrorCode.NotFound, $"There are no teams in league with id '{command.LeagueId}'.");

        var teamsDict = new Dictionary<string, Guid>();

        foreach (var item in response.Teams)
        {
            teamsDict.Add(item.Name, item.Id);
        }

        var result = await ImportMatches(teamsDict, request.CompetitionId, request.Season);

        return Result<int>.Success(result);
    }

    private async Task<int> ImportMatches(Dictionary<string, Guid> teamsDict, Guid competitionId, string season)
    {
        var path = @"C:\Users\Ilya\Desktop\matches.xlsx";

        using var stream = System.IO.File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        var numbOfMatchesAdded = 0;
        List<Match> matches = new();

        for (int nRow = 2; nRow <= nEndRow; nRow++)
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
                MatchDate = DateTime.SpecifyKind(date.Add(TimeSpan.Parse(time.ToString())),DateTimeKind.Utc),
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
