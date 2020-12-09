using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Saunter.Attributes;

namespace StreetlightsAPI
{
    public class Streetlight
    {
        /// <summary>
        /// Id of the streetlight.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Lat-Long coordinates of the streetlight.
        /// </summary>
        public double[]? Position { get; set; }
    }
    
    [ApiController]
    [Route("api/streetlights")]
    public class StreetlightsController
    {
        // Simulate a database of streetlights
        private static int StreetlightSeq = 2;
        private static readonly List<Streetlight> StreetlightDatabase = new List<Streetlight>
        {
            new Streetlight { Id = 1, Position = new [] { -36.320320, 175.485986 } },
        };
        
        private readonly IStreetlightMessageBus _streetlightMessageBus;

        public StreetlightsController(IStreetlightMessageBus streetlightMessageBus)
        {
            _streetlightMessageBus = streetlightMessageBus;
        }
        
        /// <summary>
        /// Get all streetlights
        /// </summary>
        [HttpGet]
        public IEnumerable<Streetlight> Get() => StreetlightDatabase;

        /// <summary>
        /// Add a new streetlight
        /// </summary>
        [HttpPost]
        public Streetlight Add([FromBody] Streetlight streetlight)
        {
            streetlight.Id = StreetlightSeq++;
            StreetlightDatabase.Add(streetlight);
            return streetlight;
        }

        /// <summary>
        /// Measure environmental lighting conditions for a particular streetlight.
        /// </summary>
        [HttpPost]
        [Route("{id}/measure-light")]
        public void MeasureLight([FromRoute] int id)
        {
            var streetlight = StreetlightDatabase.Single(s => s.Id == id);
            var lumens = new Random().Next(0, 3000); // Simulate "measuring" the light intensity
            
            _streetlightMessageBus.PublishLightMeasuredEvent(streetlight, lumens);
        }
    }
}
