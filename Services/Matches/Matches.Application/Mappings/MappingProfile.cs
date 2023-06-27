using AutoMapper;
using Matches.Application.Matches.Commands.CreateMatch;
using Matches.Application.Matches.Commands.UpdateMatch;
using Matches.Application.Models;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using Shared.RabbitMQ;

namespace Matches.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {   
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
            .ForMember(m => m.MatchDate, conf => conf.MapFrom(command => DateTime.SpecifyKind(command.MatchDate, DateTimeKind.Utc)))
            .ForMember(m => m.HomePlayers, conf => conf.MapFrom(command => command.HomePlayers.Select(x => new Player { Id = x }).ToList()))
            .ForMember(m => m.AwayPlayers, conf => conf.MapFrom(command => command.AwayPlayers.Select(x => new Player { Id = x }).ToList()));

        CreateMap<UpdateMatchCommand, Match>()
            .ForMember(m => m.MatchDate, conf => conf.MapFrom(command => DateTime.SpecifyKind(command.MatchDate, DateTimeKind.Utc)))
            .ForMember(m => m.Status, conf => conf.MapFrom(command => Enum.Parse<Status>(command.Status)))
            .ForMember(m => m.Stage, conf => conf.MapFrom(command => Enum.Parse<Stage>(command.Stage)))
            .ForMember(m => m.Group, conf => conf.MapFrom(command => MapGroup(command.Group)));

        CreateMap<Match, MatchSearchResponse>()
            .ForMember(m => m.HomeTeamName, conf => conf.MapFrom(m => m.HomeTeam.Name))
            .ForMember(m => m.AwayTeamName, conf => conf.MapFrom(m => m.AwayTeam.Name))
            .ForMember(m => m.Status, conf => conf.MapFrom(match => match.Status.ToString()));
    }

    public static Group? MapGroup(string? group)
    {
        return Enum.TryParse<Group>(group, out var enumGroup) ? enumGroup : null;
    }
}
