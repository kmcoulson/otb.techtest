using Newtonsoft.Json;

namespace otb.search.holidays.Entities;

public class FlightEntity
{
    public int Id { get; set; }

    public string Airline { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public int Price { get; set; }

    [JsonProperty("departure_date")]
    public DateOnly DepartureDate { get; set; }
}