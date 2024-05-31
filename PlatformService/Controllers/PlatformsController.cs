using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Interfaces;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/platforms")]
[ApiController]
public class PlatformsController: ControllerBase

{
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    
    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
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
    public ActionResult<PlatformReadDto> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        
        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
    }
}