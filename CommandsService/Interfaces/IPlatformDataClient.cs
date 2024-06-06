using CommandsService.Models;

namespace CommandsService.Interfaces;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
    
}