using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Interfaces;

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
    
    
    
}