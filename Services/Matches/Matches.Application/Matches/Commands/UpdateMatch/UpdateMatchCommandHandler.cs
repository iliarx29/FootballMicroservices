using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nest;
using Result = Matches.Application.Results.Result;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticClient;

    public UpdateMatchCommandHandler(IMatchesDbContext context, IMapper mapper, IElasticClient elasticClient)
    {
        _context = context;
        _mapper = mapper;
        _elasticClient = elasticClient;
    }

    public async Task<Result> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        match = _mapper.Map<Match>(command);

        _context.Matches.Update(match);

        await _context.SaveChangesAsync(cancellationToken);

        var updatedMatchSearchResponse = _mapper.Map<MatchSearchResponse>(match);

        //var response = await _elasticClient.UpdateAsync<MatchSearchResponse>(updatedMatchSearchResponse.Id, 
        //        u => u.Doc(updatedMatchSearchResponse), cancellationToken);

        var response = await _elasticClient.IndexDocumentAsync(updatedMatchSearchResponse, ct: cancellationToken);

        if (response.IsValid)
        {
            Console.WriteLine($"Document with id: '{updatedMatchSearchResponse.Id}' was updated");
        }

        return Result.Success();
    }
}
