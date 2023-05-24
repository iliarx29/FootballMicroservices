﻿using Matches.Application.Result;
using MediatR;

namespace Matches.Application.Matches.Commands.ImportMatches;
public record ImportMatchesCommand(Guid LeagueId, Guid SeasonId) : IRequest<Result<int>>;

