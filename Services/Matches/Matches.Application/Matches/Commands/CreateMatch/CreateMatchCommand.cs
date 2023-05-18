using Matches.Domain.Entities;
using MediatR;

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
