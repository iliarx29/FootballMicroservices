using AutoMapper;
using Matches.Domain.Entities;
using Matches.Infrastructure.Repositories;

namespace Matches.Infrastructure.Mappings;
public class MappingProfile :Profile
{
    public MappingProfile()
    {
        CreateProjection<IGrouping<GroupingObject, MatchForTeam>, Ranking>()
            .ForMember(rank => rank.TeamId, conf => conf.MapFrom(group => group.Key.TeamId))
            .ForMember(rank => rank.TeamName, conf => conf.MapFrom(group => group.Key.TeamName))
            .ForMember(rank => rank.Played, conf => conf.MapFrom(group => group.Count()))
            .ForMember(rank => rank.Wins, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored > x.GoalsConceded)))
            .ForMember(rank => rank.Loses, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored < x.GoalsConceded)))
            .ForMember(rank => rank.Draws, conf => conf.MapFrom(group => group.Count(x => x.GoalsScored == x.GoalsConceded)))
            .ForMember(rank => rank.GoalsScored, conf => conf.MapFrom(group => group.Sum(x => x.GoalsScored)))
            .ForMember(rank => rank.GoalsConceded, conf => conf.MapFrom(group => group.Sum(x => x.GoalsConceded)));
    }
}
