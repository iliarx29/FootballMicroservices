using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Commands.CreateMatch;
public record CreateMatchCommand(
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? HomeGoals,
    int? AwayGoals,
    DateTime? MatchDate,
    Guid CompetitionId,
    string Season,
    string Status,
    int? Matchday,
    string? Group,
    string Stage,
    List<Guid>? HomePlayers,
    List<Guid>? AwayPlayers) : IRequest<Match>;
