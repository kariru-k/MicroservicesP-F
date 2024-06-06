using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Interfaces;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("/api/commands/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController: ControllerBase
{
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository; 
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform([FromRoute] int platformId)
    {
        Console.WriteLine($"--> Hit the GetCommandsForPlatform: {platformId}!");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound("Platform Does Not Exist");
        }

        var commands = _repository.GetCommandsForPlatforms(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform([FromRoute] int platformId, [FromRoute] int commandId)
    {
        Console.WriteLine($"--> Hit the GetCommandsForPlatform: {platformId}!");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound("Platform Does Not Exist");
        }

        var command = _repository.GetCommand(platformId, commandId);

        if (command == null)
        {
            return NotFound("Command does not exist");
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform([FromRoute] int platformId,
        [FromBody] CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Hit the GetCommandsForPlatform: {platformId}!");

        if (!_repository.PlatformExists(platformId))
        {
            return NotFound("Platform Does Not Exist");
        }

        var command = _mapper.Map<Command>(commandCreateDto);

        _repository.CreateCommand(platformId, command);

        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
    }
    
}