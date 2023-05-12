using Matches.Application.Abstractions;
using Matches.Application.Matches.Commands.ImportMatches.Models;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Http.Json;

namespace Matches.Application.Matches.Commands.ImportMatches;
public class ImportMatchesCommandHandler : IRequestHandler<ImportMatchesCommand, int>
{
    private readonly IHttpClientFactory _client;
    private readonly IMatchesDbContext _context;

    public ImportMatchesCommandHandler(IHttpClientFactory client, IMatchesDbContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<int> Handle(ImportMatchesCommand request, CancellationToken cancellationToken)
    {
        var httpClient = _client.CreateClient();
        var url = $"https://localhost:7057/api/leagues/{request.LeagueId}/teams";

        var response = await httpClient.GetFromJsonAsync<LeagueResponse>(url);

        var teamsDict = new Dictionary<string, Guid>();

        foreach (var item in response.Teams)
        {
            teamsDict.Add(item.Name, item.Id);
        }

        var result = await ImportMatches(teamsDict, request.LeagueId, request.SeasonId);

        return result;
    }

    private async Task<int> ImportMatches(Dictionary<string, Guid> teamsDict, Guid leagueId, Guid seasonId)
    {
        var path = @"C:\Users\Ilya\Desktop\Книга1.xlsx";

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
                LeagueId = leagueId,
                SeasonId = seasonId,
                MatchDate = DateTime.SpecifyKind(date.Add(TimeSpan.Parse(time.ToString())),DateTimeKind.Utc),
                Status = Status.Finished
            };

            matches.Add(match);
            numbOfMatchesAdded++;
        }

        await _context.Matches.AddRangeAsync(matches);

        await _context.SaveChangesAsync();

        return numbOfMatchesAdded;
    }
}
