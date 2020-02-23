using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TemplateSolution.App;
using TemplateSolution.Infrastructure;
using TemplateSolution.WebApi.ViewModels;

namespace TemplateSolution.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly RawDataManager _app;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IOptions<AppSetting> _appSetting;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<AppSetting> appSetting, RawDataManager app)
        {
            _logger = logger;
            _appSetting = appSetting;
            _app = app;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var a = _app.Get(1).MapTo<RawDataVM>();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
