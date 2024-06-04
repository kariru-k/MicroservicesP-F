using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException(), isProd);
            
        }
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
        
        if (isProd)
        {
            Console.WriteLine("Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not run migrations: {e.Message}");
                throw;
            }
        }
        
        if (!context.Platforms.Any())
        {
           Console.WriteLine("--> Seeding data..."); 
           
           context.Platforms.AddRange(
               new Platform() {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
               new Platform() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
               new Platform() {Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
            );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> Data already present!");
        }
    }
}