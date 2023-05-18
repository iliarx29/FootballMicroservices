using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public record UpdateMatchCommand(
    Guid Id,
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
    List<Guid>? AwayPlayers) : IRequest<Result>;