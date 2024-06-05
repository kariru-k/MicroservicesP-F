using PlatformService.Dtos;

namespace PlatformService.Interfaces;

public interface IMessageBusClient

{
    void PublishNewPlatform(PlatformPublishDto platformPublishDto);
}