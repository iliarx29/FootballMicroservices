using FluentValidation;
using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;

namespace Matches.Application.Players.Commands;
public record CreatePlayerCommand(
    string Name,
    string CountryName,
    Guid? TeamId,
    int? ShirtNumber,
    DateTime DateOfBirth,
    string Position) : IRequest<Result<Player>>;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<Player>>
{
    private readonly IMatchesDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerCommandHandler(IMatchesDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Player>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        var player = new Player()
        {
            Name = command.Name,
            CountryName = command.CountryName,
            DateOfBirth = DateTime.SpecifyKind(command.DateOfBirth, DateTimeKind.Utc),
            Position = Enum.Parse<Position>(command.Position),
            ShirtNumber = command.ShirtNumber,
            TeamId = command.TeamId
        };

        await _context.Players.AddAsync(player, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Player>.Success(player);
    }
}

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.CountryName).NotNull().NotEmpty();
        RuleFor(x => x.Position).IsEnumName(typeof(Position));
    }
}

