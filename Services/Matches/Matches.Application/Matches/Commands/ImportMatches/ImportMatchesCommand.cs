using Matches.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Matches.Application.Matches.Commands.ImportMatches;
public record ImportMatchesCommand(Guid CompetitionId, string Season, IFormFile File) : IRequest<Result<int>>;

