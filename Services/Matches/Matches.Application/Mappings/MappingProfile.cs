using AutoMapper;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Matches.Common.Responses;
using Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using Shared.RabbitMQ;

namespace Matches.Application.Mappings;
public class MappingProfile : Profile
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

        CreateMap<TeamCreatedEvent, Team>();

        CreateMap<Match, MatchResponse>()
            .ForMember(res => res.HomeTeamName, conf => conf.MapFrom(m => m.HomeTeam.Name))
            .ForMember(res => res.AwayTeamName, conf => conf.MapFrom(m => m.AwayTeam.Name))
            .ForMember(res => res.Stage, conf => conf.MapFrom(m => m.Stage.ToString()))
            .ForMember(res => res.Group, conf => conf.MapFrom(m => m.Group.ToString()));

        CreateMap<CreateMatchCommand, Match>()
            .ForMember(m => m.Status, conf => conf.MapFrom(command => Enum.Parse<Status>(command.Status)))
            .ForMember(m => m.Stage, conf => conf.MapFrom(command => Enum.Parse<Stage>(command.Stage)))
            .ForMember(m => m.Group, conf => conf.MapFrom(command => MapGroup(command.Group)))
            .ForMember(m => m.HomePlayers, conf => conf.MapFrom(command => command.HomePlayers.Select(x => new Player { Id = x }).ToList()))
            .ForMember(m => m.AwayPlayers, conf => conf.MapFrom(command => command.AwayPlayers.Select(x => new Player { Id = x }).ToList()));

        CreateMap<UpdateMatchCommand, Match>()
            .ForMember(m => m.Status, conf => conf.MapFrom(command => Enum.Parse<Status>(command.Status)))
            .ForMember(m => m.Stage, conf => conf.MapFrom(command => Enum.Parse<Stage>(command.Stage)))
            .ForMember(m => m.Group, conf => conf.MapFrom(command => MapGroup(command.Group)));
    }

    public static Group? MapGroup(string? group)
    {
        return Enum.TryParse<Group>(group, out var enumGroup) ? enumGroup : null;
    }
}
