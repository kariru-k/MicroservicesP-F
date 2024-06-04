using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Mappers;

public class CommandsMapper: Profile
{
    public CommandsMapper()
    {
        //Source --> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
    }
    
}