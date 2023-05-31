using FluentValidation;
using Matches.Domain.Entities.Enums;

namespace Matches.Application.Matches.Commands.CreateMatch;

public sealed class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.HomeTeamId).NotEmpty();
        RuleFor(x => x.AwayTeamId).NotEmpty();
        RuleFor(x => x.Season).NotEmpty();
        RuleFor(x => x.CompetitionId).NotEmpty();
        RuleFor(x => x.Status).IsEnumName(typeof(Status));
        RuleFor(x => x.Stage).IsEnumName(typeof(Stage));
    }
}