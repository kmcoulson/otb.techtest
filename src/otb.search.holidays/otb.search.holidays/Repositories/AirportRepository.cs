namespace otb.search.holidays.Repositories
{
    public interface IAirportRepository
    {
        List<string> GetAirportCodesForCity(string city);
    }

    public class AirportRepository : IAirportRepository
    {
        private readonly Dictionary<string, List<string>> _airportData = new()
        {
            {
                "london",
                new List<string> { "LTN", "LGW" }
            }
        };

        public List<string> GetAirportCodesForCity(string city)
        {
            return _airportData.ContainsKey(city.ToLower()) ? _airportData[city.ToLower()] : new List<string>();
        }
    }
}
