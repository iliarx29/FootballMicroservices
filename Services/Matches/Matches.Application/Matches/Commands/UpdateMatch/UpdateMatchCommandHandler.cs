using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Commands.UpdateMatch;
public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IElasticService _elasticService;

    public UpdateMatchCommandHandler(IMatchesRepository matchesRepository, IMapper mapper, IElasticService elasticService, IUnitOfWork unitOfWork)
    {
        _matchesRepository = matchesRepository;
        _mapper = mapper;
        _elasticService = elasticService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _matchesRepository.GetMatchByIdAsync(command.Id, cancellationToken);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        match = _mapper.Map<Match>(command);

        _matchesRepository.UpdateMatch(match);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedMatchSearchResponse = _mapper.Map<MatchSearchResponse>(match);

        await _elasticService.AddDocument(updatedMatchSearchResponse, cancellationToken);

        return Result.Success();
    }
}
