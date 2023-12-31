﻿using Newtonsoft.Json;

namespace otb.search.holidays.Entities;

public class FlightEntity
{
    public int Id { get; set; }

    public string Airline { get; set; } = string.Empty;

    public string From { get; set; } = string.Empty;

    public string To { get; set; } = string.Empty;

    public int Price { get; set; }

    [JsonProperty("departure_date")]
    public DateOnly DepartureDate { get; set; }
}