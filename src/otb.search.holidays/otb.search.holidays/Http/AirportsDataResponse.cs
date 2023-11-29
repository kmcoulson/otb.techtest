namespace otb.search.holidays.Http
{
    public class AirportsDataResponse
    {
        public List<Airport> Airports { get; set; } = new();
    }
    public class Airport
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Iata { get; set; } = string.Empty;
    }
}
