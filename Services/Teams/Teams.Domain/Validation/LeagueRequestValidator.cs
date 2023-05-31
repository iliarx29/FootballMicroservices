using FluentValidation;
using Teams.Domain.Models;

namespace Teams.Domain.Validation;
public class LeagueRequestValidator : AbstractValidator<CompetitionRequest>
{
    public LeagueRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Area).NotNull().NotEmpty();
        RuleFor(x => x.Code).NotNull().NotEmpty();
    }
}
