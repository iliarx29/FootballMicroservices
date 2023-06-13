﻿using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchById;
public record GetMatchByIdQuery(Guid Id) : IRequest<Result<MatchResponse>>;
