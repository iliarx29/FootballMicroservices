using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result>
{
    private readonly IMatchesDbContext _context;

    public UpdateMatchCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        match.HomeTeamId = command.HomeTeamId;
        match.AwayTeamId = command.AwayTeamId;
        match.HomeGoals = command.HomeGoals;
        match.AwayGoals = command.AwayGoals;
        match.MatchDate = command.MatchDate;
        match.LeagueId = command.LeagueId;
        match.Status = Enum.Parse<Status>(command.Status);
        match.SeasonId = command.SeasonId;
        match.Round = command.Round;

        _context.Matches.Update(match);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
