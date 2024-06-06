using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Mappers;

public class CommandsMapper: Profile
{
    public CommandsMapper()
    {
        //Source --> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(destination => destination.ExternalId, opt => opt.MapFrom(src => src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(destination => destination.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(destination => destination.Commands, opt => opt.Ignore());
    }
    
}