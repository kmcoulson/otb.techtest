using Newtonsoft.Json;

namespace otb.search.holidays.Entities;

public class HotelEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;


    [JsonProperty("arrival_date")]
    public DateOnly ArrivalDate { get; set; }


    [JsonProperty("price_per_night")]
    public int PricePerNight { get; set; }


    [JsonProperty("local_airports")]
    public List<string> LocalAirports { get; set; } = new();

    public int Nights { get; set; }
}
