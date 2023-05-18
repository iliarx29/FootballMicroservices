using FluentValidation;
using Teams.Domain.Models;

namespace Teams.Domain.Validation;
public class LeagueRequestValidator : AbstractValidator<LeagueRequest>
{
    public LeagueRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.CountryName).NotNull().NotEmpty();
        RuleFor(x => x.Code).NotNull().NotEmpty();
    }
}
