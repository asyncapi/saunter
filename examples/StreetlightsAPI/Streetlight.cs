using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StreetlightsAPI
{
    public class Streetlight
    {
        public int Id { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }

    public class LightMeasuredEvent
    {
        public int Id { get; set; }

        public int Lumens { get; set; }

        public DateTime SentAt { get; set; }
    }
    
    [ApiController]
    [Route("[controller]")]
    public class StreetlightsController : ControllerBase
    {
        private readonly IStreetlightRepository _streetlightRepository;

        public StreetlightsController(IStreetlightRepository streetlightRepository)
        {
            _streetlightRepository = streetlightRepository;
        }

        [HttpGet]
        public IEnumerable<Streetlight> Get() => _streetlightRepository.GetStreetlights();

        [HttpPost]
        public Streetlight Add([FromBody] Streetlight streetlight)
        {
            _streetlightRepository.AddStreetlight(streetlight);
            return streetlight;
        }

        [HttpGet]
        [Route("{id}/measure-light")]
        public int MeasureLight([FromRoute] int id)
        {
            var streetlight = _streetlightRepository.GetStreetlightById(id);
            var lumens = _streetlightRepository.MeasureLight(streetlight);
            return lumens;
        }
    }
    
    public interface IStreetlightRepository
    {
        IEnumerable<Streetlight> GetStreetlights();
        Streetlight GetStreetlightById(int id);
        void AddStreetlight(Streetlight streetlight);
        int MeasureLight(Streetlight streetlight);
    }
}
