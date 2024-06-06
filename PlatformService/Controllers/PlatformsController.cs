using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Interfaces;
using PlatformService.Models;
using PlatformService.SyncDataServices.http;

namespace PlatformService.Controllers;

[Route("api/platforms")]
[ApiController]
public class PlatformsController: ControllerBase

{
    private readonly ICommandDataClient _commandDataClient;
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> getting platforms!");
        
        var platforms = _platformRepo.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById([FromRoute] int id)
    {
        Console.WriteLine("--> Looking for the platform");

        var platform = _platformRepo.GetPlatformById(id);

        if (platform == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        
        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            throw;
        }

        try
        {
            var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
            platformPublishDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            throw;
        }
        
        return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
    }
}