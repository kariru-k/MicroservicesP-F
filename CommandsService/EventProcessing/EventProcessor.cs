using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Enums;
using CommandsService.Interfaces;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor: IEventProcessor
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining the Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType!.Event)
        {
            case "Platform_Published":
            {
                Console.WriteLine("Platform Published Event Detected");
                return EventType.PlatformPublished;
            }
            default:
            {
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
            }
        }
        
    }
    
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
            {
                AddPlatform(message);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    Console.WriteLine("Platform added");
                }
                else
                {
                    Console.WriteLine("--> Platform already exists...");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not add platform to database: {e.Message}");
                throw;
            }
        }
    }
            
}