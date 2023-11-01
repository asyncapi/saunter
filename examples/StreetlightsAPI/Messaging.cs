using System.Text.Json;

using Microsoft.Extensions.Logging;

using Saunter.Attributes;

namespace StreetlightsAPI;

public interface IStreetlightMessageBus
{
    void PublishLightMeasurement(LightMeasuredEvent lightMeasuredEvent);
}

public class StreetlightMessageBus : IStreetlightMessageBus
{
    private const string SubscribeLightMeasuredTopic = "subscribe/light/measured";

    private readonly ILogger _logger;

    public StreetlightMessageBus(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger("Streetlight");
    }

    [SubscribeOperation<LightMeasuredEvent>(SubscribeLightMeasuredTopic, "Light", Summary = "Subscribe to environmental lighting conditions for a particular streetlight.", ChannelServers = new[] { "mosquitto" })]
    public void PublishLightMeasurement(LightMeasuredEvent lightMeasuredEvent)
    {
        string payload = JsonSerializer.Serialize(lightMeasuredEvent);

        // Simulate publishing a message to the channel.
        // In reality this would call some kind of pub/sub client library and publish.
        // e.g. mqttClient.PublishAsync(message);

        _logger.LogInformation("Publishing message {Payload} to test.mosquitto.org/{Topic}", payload, SubscribeLightMeasuredTopic);
    }
}