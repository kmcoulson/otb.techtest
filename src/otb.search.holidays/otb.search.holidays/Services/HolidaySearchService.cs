using otb.search.holidays.Dtos;
using otb.search.holidays.Repositories;

namespace otb.search.holidays.Services
{
    public interface IHolidaySearchService
    {
        Task<IEnumerable<HolidaySearchResultDto>> Search(string from, string to, DateOnly date, int nights);
    }

    public class HolidaySearchService : IHolidaySearchService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IHotelRepository _hotelRepository;

        public HolidaySearchService(IFlightRepository flightRepository, IHotelRepository hotelRepository)
        {
            _flightRepository = flightRepository;
            _hotelRepository = hotelRepository;
        }

        public Task<IEnumerable<HolidaySearchResultDto>> Search(string from, string to, DateOnly date, int nights)
        {
            throw new NotImplementedException();
        }
    }
}
