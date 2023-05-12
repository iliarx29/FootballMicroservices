﻿using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Matches.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchById;
public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, Match>
{
    private readonly IMatchesDbContext _context;

    public GetMatchByIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Match> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
    {
        var match = await _context.Matches
            .Select(x => new Match
            {
                Id = x.Id, 
                HomeTeamId = x.HomeTeamId, 
                HomeGoals = x.HomeGoals, 
                AwayGoals = x.AwayGoals, 
                AwayTeamId = x.AwayTeamId, 
                MatchDate = x.MatchDate,
                Status = x.Status, 
                Round = x.Round,
                LeagueId = x.LeagueId, 
                SeasonId = x.SeasonId,
                HomePlayers = x.HomePlayers.Select(x => new Player { Id = x.Id, Name = x.Name }).ToList(),
                AwayPlayers = x.AwayPlayers.Select(x => new Player { Id = x.Id, Name = x.Name }).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (match is null)
            throw new NotFoundException($"Match with id: '{query.Id}' doesn't exist.");

        return match;
    }
}
