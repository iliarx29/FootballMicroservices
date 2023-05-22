using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Matches.Application.Players.Commands;
public record CreatePlayerCommand(
    string Name,
    string CountryName,
    Guid? TeamId,
    int? ShirtNumber,
    DateTime? DateOfBirth,
    string Position) : IRequest<Player>;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Player>
{
    private readonly IMatchesDbContext _context;

    public CreatePlayerCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Player> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        var player = new Player()
        {
            Name = command.Name,
            CountryName = command.CountryName,
            DateOfBirth = command.DateOfBirth,
            Position = Enum.Parse<Position>(command.Position),
            ShirtNumber = command.ShirtNumber,
            TeamId = command.TeamId
        };

        await _context.Players.AddAsync(player, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return player;
    }
}

