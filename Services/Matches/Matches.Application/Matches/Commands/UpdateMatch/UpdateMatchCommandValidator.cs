using FluentValidation;
using Matches.Domain.Entities.Enums;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.HomeTeamId).NotNull().NotEmpty();
        RuleFor(x => x.AwayTeamId).NotNull().NotEmpty();
        RuleFor(x => x.Season).NotNull().NotEmpty();
        RuleFor(x => x.CompetitionId).NotNull().NotEmpty();
        RuleFor(x => x.Status).NotNull().IsEnumName(typeof(Status));
        RuleFor(x => x.Stage).NotNull().IsEnumName(typeof(Status));
    }
}
