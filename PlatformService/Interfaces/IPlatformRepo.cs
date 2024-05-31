using PlatformService.Models;

namespace PlatformService.Interfaces;

public interface IPlatformRepo
{

    public bool SaveChanges();
    
    IEnumerable<Platform> GetAllPlatforms();

    Platform? GetPlatformById(int id);

    void CreatePlatform(Platform platform);
    

}