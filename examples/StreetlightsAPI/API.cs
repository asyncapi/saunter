using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AsyncApi.Net.Generator.Attributes;

namespace StreetlightsAPI;

public class Streetlight
{
    /// <summary>
    /// Id of the streetlight.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Lat-Long coordinates of the streetlight.
    /// </summary>
    public double[] Position { get; set; }

    /// <summary>
    /// History of light intensity measurements
    /// </summary>
    public List<KeyValuePair<DateTime, int>> LightIntensity { get; set; }
}

public class LightMeasuredEvent
{
    /// <summary>
    /// Id of the streetlight.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Light intensity measured in lumens.
    /// </summary>
    public int Lumens { get; set; }

    /// <summary>
    /// Light intensity measured in lumens.
    /// </summary>
    public DateTime SentAt { get; set; }
}

[ApiController]
[Route("")]
public class StreetlightsController
{
    private const string PublishLightMeasuredTopic = "publish/light/measured";

    // Simulate a database of streetlights
    private static int StreetlightSeq = 2;
    private static readonly List<Streetlight> StreetlightDatabase =
    [
        new Streetlight { Id = 1, Position = new [] { -36.320320, 175.485986 }, LightIntensity = [] },
    ];

    private readonly IStreetlightMessageBus _streetlightMessageBus;
    private readonly ILogger _logger;

    public StreetlightsController(IStreetlightMessageBus streetlightMessageBus, ILoggerFactory loggerFactory)
    {
        _streetlightMessageBus = streetlightMessageBus;
        _logger = loggerFactory.CreateLogger<StreetlightsController>();
    }

    /// <summary>
    /// Get all streetlights
    /// </summary>
    [HttpGet]
    [Route("api/streetlights")]
    public IEnumerable<Streetlight> Get() => StreetlightDatabase;

    /// <summary>
    /// Add a new streetlight
    /// </summary>
    [HttpPost]
    [Route("api/streetlights")]
    public Streetlight Add([FromBody] Streetlight streetlight)
    {
        streetlight.Id = StreetlightSeq++;
        StreetlightDatabase.Add(streetlight);
        return streetlight;
    }

    /// <summary>
    /// Inform about environmental lighting conditions for a particular streetlight.
    /// </summary>
    [HttpPost]
    [Route(PublishLightMeasuredTopic)]
    [PublishOperation<LightMeasuredEvent>(PublishLightMeasuredTopic, "Light", ChannelServers = new[] { "webapi" })]
    [PublishOperation<LightMeasuredEvent>(PublishLightMeasuredTopic, "Light", ChannelServers = new[] { "webapi" }, DocumentName = "Foo")]
    public void MeasureLight([FromBody] LightMeasuredEvent lightMeasuredEvent)
    {
        lightMeasuredEvent.SentAt = DateTime.Now;

        string payload = JsonSerializer.Serialize(lightMeasuredEvent);

        _logger.LogInformation("Received message on {Topic} with payload {Payload} ", PublishLightMeasuredTopic, payload);

        Streetlight streetlight = StreetlightDatabase.SingleOrDefault(s => s.Id == lightMeasuredEvent.Id);
        if (streetlight != null)
        {
            streetlight.LightIntensity.Add(new(lightMeasuredEvent.SentAt, lightMeasuredEvent.Lumens));

            // Re-publish messages we receive
            _streetlightMessageBus.PublishLightMeasurement(lightMeasuredEvent);
        }
    }
}
