﻿using Matches.Application.Result;
using MediatR;

namespace Matches.Application.Matches.Commands.ImportMatches;
public record ImportMatchesCommand(Guid CompetitionId, string Season) : IRequest<Result<int>>;

