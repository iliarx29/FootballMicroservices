using Matches.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matches.Application.Matches.Commands.DeleteMatch;
public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand>
{
    private readonly IMatchesDbContext _context;

    public DeleteMatchCommandHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if(match is null)
            throw new ArgumentNullException($"Match with id: '{command.Id}'is null");

        _context.Matches.Remove(match);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
