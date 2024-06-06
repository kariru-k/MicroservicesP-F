using CommandsService.Interfaces;
using CommandsService.Models;

namespace CommandsService.Data;

public class PrepDB
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = grpcClient!.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding New Platforms from Platform Service");

        foreach (var platform in platforms)
        {
            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                Console.WriteLine($"--> Found a new Platform: {platform}");
                repo.CreatePlatform(platform);
            }

            repo.SaveChanges();
        }
    }
    
    
}