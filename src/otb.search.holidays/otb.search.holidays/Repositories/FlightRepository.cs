using Newtonsoft.Json;
using otb.search.holidays.Entities;

namespace otb.search.holidays.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<FlightEntity>> GetAll();
    }

    public class FlightRepository : IFlightRepository
    {
        public Task<IEnumerable<FlightEntity>> GetAll()
        {
            var json = File.ReadAllText("Data/flights.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<FlightEntity>>(json);

            if (data == null) throw new Exception("An error occurred parsing data for Flights");

            return Task.FromResult(data);
        }
    }
}
