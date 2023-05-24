using Matches.Application.Abstractions;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Commands.DeleteMatch;
public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result>
{
    private readonly IMatchesDbContext _context;

    public DeleteMatchCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        _context.Matches.Remove(match);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
