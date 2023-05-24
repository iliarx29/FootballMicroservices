using Matches.Application.Abstractions;
using Matches.Application.Matches.Commands.ImportMatches.Models;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Http.Json;

namespace Matches.Application;
public class ImportDataReccuringJob
{
    private readonly IHttpClientFactory _client;
    private readonly IMatchesDbContext _context;

    public ImportDataReccuringJob(IHttpClientFactory client, IMatchesDbContext context)
    {
        _client = client;
        _context = context;
    }

    //public async Task Handle(Guid leagueId, string season)
    //{
    //    var httpClient = _client.CreateClient();
    //    var url = $"https://localhost:7057/api/competitions/{leagueId}/teams";

    //    var response = await httpClient.GetFromJsonAsync<CompetitionResponse>(url);

    //    if (response is null)
    //        throw new ArgumentNullException();

    //    var teamsDict = new Dictionary<string, Guid>();

    //    foreach (var item in response.Teams)
    //    {
    //        teamsDict.Add(item.Name, item.Id);
    //    }

    //    await ImportMatches(teamsDict, leagueId, season);
    //}

    private async Task ImportMatches(Guid competitionId, string season)
    {
        var path = @"C:\Users\Ilya\Desktop\matches.xlsx";

        using var stream = System.IO.File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        List<Match> matchesToAdd = new();

        var teamsDict = new Dictionary<string, Guid>();

        var teams = await _context.Teams.ToListAsync();

        foreach (var item in teams)
        {
            teamsDict.Add(item.Name, item.Id);
        }

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
                MatchDate = DateTime.SpecifyKind(date.Add(TimeSpan.Parse(time.ToString())), DateTimeKind.Utc),
                Status = Status.Finished,
                Stage = Stage.Regular_Season
            };

            matchesToAdd.Add(match);
        }

        await _context.Matches.BulkInsertAsync(matchesToAdd, options =>
        {
            options.InsertIfNotExists = true;
            options.ColumnPrimaryKeyExpression = x => new { x.HomeTeamId, x.AwayTeamId, x.MatchDate };
        });

        //await _context.Matches.AddRangeAsync(matchesToAdd);

        //await _context.SaveChangesAsync();
    }

}
