using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Matches.Application.Players.Commands;
public record ImportPlayersCommand() : IRequest<Result<int>>;

public class ImportPlayersCommandHandler : IRequestHandler<ImportPlayersCommand, Result<int>>
{
    private readonly IMatchesDbContext _context;
    public ImportPlayersCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(ImportPlayersCommand request, CancellationToken cancellationToken)
    {
        var teams = await _context.Teams.ToListAsync(cancellationToken);

        var teamsDict = new Dictionary<string, Guid>();

        foreach (var item in teams)
        {
            teamsDict.Add(item.Name, item.Id);
        }

        var path = @"C:\Users\Ilya\Desktop\Players.xlsx";

        using var stream = File.OpenRead(path);
        using var excelPackage = new ExcelPackage(stream);

        var worksheet = excelPackage.Workbook.Worksheets[0];
        var nEndRow = worksheet.Dimension.End.Row;

        var playersCount = await _context.Players.CountAsync(cancellationToken);

        var numbOfPlayersAdded = 0;
        List<Player> players = new();

        for (int nRow = playersCount + 2; nRow <= nEndRow; nRow++)
        {
            var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

            var name = row[nRow, 1].GetValue<string>();
            var country = row[nRow, 2].GetValue<string>();
            var teamName = row[nRow, 3].GetValue<string>();
            var shirtNumber = row[nRow, 4].GetValue<int>();
            var position = row[nRow, 6].GetValue<string>();

            teamsDict.TryGetValue(teamName, out Guid teamId);

            var player = new Player
            {
                TeamId = teamId,
                Name = name,
                CountryName = country,
                ShirtNumber = shirtNumber,
                DateOfBirth = DateTime.SpecifyKind(new DateTime(), DateTimeKind.Utc),
                Position = Enum.Parse<Position>(position)
            };

            players.Add(player);
            numbOfPlayersAdded++;
        }

        await _context.Players.AddRangeAsync(players, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(numbOfPlayersAdded);
    }
}

