using FluentValidation;
using Teams.Domain.Models;

namespace Teams.Domain.Validation;
public class TeamRequestValidator : AbstractValidator<TeamRequest>
{
    public TeamRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Stadium).NotNull().NotEmpty();
        RuleFor(x => x.Code).NotNull().NotEmpty().Length(3);
        RuleFor(x => x.City).NotNull().NotEmpty();
        RuleFor(x => x.CountryName).NotNull().NotEmpty();
    }
}
