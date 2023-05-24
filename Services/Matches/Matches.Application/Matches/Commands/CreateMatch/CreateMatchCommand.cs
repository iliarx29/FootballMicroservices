using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matches.Application.Matches.Commands.CreateMatch;
public record CreateMatchCommand(
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? HomeGoals,
    int? AwayGoals,
    DateTime? MatchDate,
    Guid LeagueId,
    Guid SeasonId,
    string Status,
    int Round,
    List<Guid>? HomePlayers,
    List<Guid>? AwayPlayers) : IRequest<Match>;
