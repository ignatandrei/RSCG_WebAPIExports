using System.Diagnostics;

namespace TestWebAPI;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
[DebuggerDisplay("{Summary}")]
public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF
    {
        get
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    } 

    public string? Summary { get; set; }

    public WeatherForecast? NewItem
    {
        get; set;
    }
}