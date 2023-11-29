using System.Reflection.Emit;
using System.Text.RegularExpressions;
using otb.search.holidays.Dtos;
using otb.search.holidays.Mappers;
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
        private readonly IAirportRepository _airportRepository;

        public HolidaySearchService(IFlightRepository flightRepository, IHotelRepository hotelRepository, IAirportRepository airportRepository)
        {
            _flightRepository = flightRepository;
            _hotelRepository = hotelRepository;
            _airportRepository = airportRepository;
        }

        public async Task<IEnumerable<HolidaySearchResultDto>> Search(string from, string to, DateOnly date, int nights)
        {
            var flightsTask = Task.Run(() => _flightRepository.GetAll());
            var hotelsTask = Task.Run(() => _hotelRepository.GetAll());

            await Task.WhenAll(flightsTask, hotelsTask);

            var allFlights = flightsTask.Result;
            var allHotels = hotelsTask.Result;

            var fromCodes = new List<string>();
            var toCodes = new List<string>();

            if (from.StartsWith("ANY ", StringComparison.CurrentCultureIgnoreCase))
            {
                var anyCityRegexPattern = @"(?i)\b(?:Any|Airport)\b";
                var cityName = Regex.Replace(from, anyCityRegexPattern, "").Trim();
                var airportCodes = _airportRepository.GetAirportCodesForCity(cityName);
                fromCodes.AddRange(airportCodes);
            }
            else if (from.Equals("ANY", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                fromCodes.Add(from);
            }

            if (to.StartsWith("ANY ", StringComparison.CurrentCultureIgnoreCase))
            {
                var anyCityRegexPattern = @"(?i)\b(?:Any|Airport)\b";
                var cityName = Regex.Replace(to, anyCityRegexPattern, "").Trim();
                var airportCodes = _airportRepository.GetAirportCodesForCity(cityName);
                toCodes.AddRange(airportCodes);

            }
            else if (from.Equals("ANY", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                toCodes.Add(to);
            }

            var validFlights = allFlights
                .Where(x =>
                    (!fromCodes.Any() || fromCodes.Any(f => f.Equals(x.From, StringComparison.OrdinalIgnoreCase)))
                    && (!toCodes.Any() || toCodes.Any(t => t.Equals(x.To, StringComparison.OrdinalIgnoreCase)))
                    && x.DepartureDate.Equals(date))
                .ToList();

            var validHotels = allHotels
                .Where(x =>
                    (!toCodes.Any() || x.LocalAirports.Intersect(toCodes, StringComparer.CurrentCultureIgnoreCase).Any())
                    && x.ArrivalDate.Equals(date))
                .ToList();

            var results = new List<HolidaySearchResultDto>();

            foreach (var flight in validFlights)
            {
                foreach (var hotel in validHotels)
                {
                    results.Add(new HolidaySearchResultDto
                    {
                        Flight = flight.ToDto(),
                        Hotel = hotel.ToDto()
                    });
                }
            }

            return results.OrderBy(x => x.TotalPrice);
        }
    }
}
