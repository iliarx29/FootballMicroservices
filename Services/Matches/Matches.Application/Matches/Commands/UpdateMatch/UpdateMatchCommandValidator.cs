using FluentValidation;
using Matches.Domain.Entities;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public  class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.HomeTeamId).NotNull().NotEmpty();
        RuleFor(x => x.AwayTeamId).NotNull().NotEmpty();
        RuleFor(x => x.Round).NotNull().NotEmpty();
        RuleFor(x => x.SeasonId).NotNull().NotEmpty();
        RuleFor(x => x.LeagueId).NotNull().NotEmpty();
        RuleFor(x => x.Status).NotNull().IsEnumName(typeof(Status));
    }
}
