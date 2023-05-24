﻿using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Commands.CreateMatch;
public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<Match>>
{
    private readonly IMatchesDbContext _context;

    public CreateMatchCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Match>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = new Match()
        {
            HomeTeamId = command.HomeTeamId,
            AwayTeamId = command.AwayTeamId,
            HomeGoals = command.HomeGoals,
            AwayGoals = command.AwayGoals,
            MatchDate = command.MatchDate,
            LeagueId = command.LeagueId,
            SeasonId = command.SeasonId,
            Round = command.Round,
            Status = Enum.Parse<Status>(command.Status),
            HomePlayers = command.HomePlayers.Select(x => new Player { Id = x }).ToList(),
            AwayPlayers = command.AwayPlayers.Select(x => new Player { Id = x }).ToList()
        };

        _context.Matches.Attach(match);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Match>.Success(match);
    }
}
