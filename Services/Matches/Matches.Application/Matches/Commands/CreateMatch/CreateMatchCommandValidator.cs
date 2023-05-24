using FluentValidation;
using Matches.Domain.Entities;

namespace Matches.Application.Matches.Commands.CreateMatch;

public sealed class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.HomeTeamId).NotEmpty();
        RuleFor(x => x.AwayTeamId).NotEmpty();
        RuleFor(x => x.Round).NotEmpty().NotNull();
        RuleFor(x => x.SeasonId).NotEmpty();
        RuleFor(x => x.LeagueId).NotEmpty();
        RuleFor(x => x.Status).IsEnumName(typeof(Status));
    }
}