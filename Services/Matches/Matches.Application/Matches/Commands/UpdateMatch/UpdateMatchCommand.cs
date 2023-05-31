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
    Guid CompetitionId,
    string Season,
    string Status,
    string Stage,
    string? Group,
    int? Matchday) : IRequest<Result>;
