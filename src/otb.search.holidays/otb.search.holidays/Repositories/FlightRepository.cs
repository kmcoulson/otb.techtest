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

            throw new NotImplementedException();
        }
    }
}
