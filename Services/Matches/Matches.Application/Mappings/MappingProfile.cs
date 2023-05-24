using AutoMapper;
using Matches.Application.Matches.Queries.GetStandingsByLeagueId;
using Matches.Domain.Entities;

namespace Matches.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateProjection<IGrouping<Guid, MatchForTeam>, Ranking>()
                .ForMember(rank => rank.TeamId, conf => conf.MapFrom(group => group.Key))
                .ForMember(rank => rank.Played, conf => conf.MapFrom(group => group.Count()))
                .ForMember(rank => rank.Wins, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored > x.GoalsConceded)))
                .ForMember(rank => rank.Loses, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored < x.GoalsConceded)))
                .ForMember(rank => rank.Draws, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored == x.GoalsConceded)))
                .ForMember(rank => rank.GoalsScored, conf => conf.MapFrom(group => group.Sum(x => x.GoalsScored)))
                .ForMember(rank => rank.GoalsConceded, conf => conf.MapFrom(group => group.Sum(x => x.GoalsConceded)));
    }
}
