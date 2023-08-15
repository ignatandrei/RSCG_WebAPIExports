using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public int Count()
        {
            return 10;
        }
        [HttpPost]
        public WeatherForecast[] TestNew()
        {
            return GetWeatherForecast().ToArray();
        }
        [HttpDelete]
        public Results<Ok<WeatherForecast[]>,NotFound<string>> TestResults()
        {
            var data = GetWeatherForecast().ToArray();
            if ((data?.Length ?? 0) == 0) return TypedResults.NotFound("cannot find");
            return TypedResults.Ok(data);
        }
        [HttpGet]
        public Person GetPerson(int id)
        {
            return new Person() { Id = id, Name = "test" + id };
        }
        
        [HttpPut]
        public ActionResult<WeatherForecast[]> TestActionResult()
        {
            return GetWeatherForecast().ToArray();
        }
        [HttpGet()]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                NewItem = new WeatherForecast()
                {
                    TemperatureC=100
                }
            })
            .ToArray();
        }
    }

}