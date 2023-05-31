using AutoMapper;
using Teams.Domain.Models;
using Teams.Infrastructure.Entities;

namespace Teams.Domain.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeamResponse, Team>().ReverseMap();
        CreateMap<TeamRequest, Team>().ReverseMap();
        CreateMap<CompetitionResponse, Competition>().ReverseMap();
        CreateMap<CompetitionRequest, Competition>().ReverseMap();
    }
}
