using Matches.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matches.Application.Matches.Queries.GetMatchesByLeagueId;
public record GetMatchesByLeagueIdQuery(Guid LeagueId): IRequest<IEnumerable<Match>>;
