using otb.search.holidays.Entities;
using otb.search.holidays.Enums;

namespace otb.search.holidays.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<FlightEntity>> GetAll();
    }

    public class FlightRepository : BaseDataRepository, IFlightRepository
    {
        public Task<IEnumerable<FlightEntity>> GetAll()
        {
            var data = GetData<FlightEntity>(DataType.Flights);
            return Task.FromResult(data);
        }
    }
}
